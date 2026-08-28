using System.Collections.Generic;

namespace Combat.Actor {
    public static class ObstacleAvoidUtils {
        private static readonly FloatF DetectRadius = 50;
        private static readonly FloatF PredictFrameCount = 8;

        private readonly struct Candidate {
            public readonly FloatF Cos;
            public readonly FloatF Sin;

            public Candidate(FloatF cos, FloatF sin) {
                Cos = cos;
                Sin = sin;
            }
        }

        private readonly struct Obstacle {
            public readonly Actor Actor;
            public readonly Vector3F ExpectMove;

            public Obstacle(Actor actor, Vector3F expectMove) {
                Actor = actor;
                ExpectMove = expectMove;
            }
        }

        // 优先保持原方向；需要避让时双方都优先向各自右侧移动，避免迎面时选择同一侧。
        private static readonly Candidate[] Candidates = {
            new Candidate("1", "0"),
            new Candidate("0.866025", "0.5"),
            new Candidate("0.866025", "-0.5"),
            new Candidate("0.5", "0.866025"),
            new Candidate("0.5", "-0.866025"),
            new Candidate("0", "1"),
            new Candidate("0", "-1"),
            new Candidate("-0.5", "0.866025"),
            new Candidate("-0.5", "-0.866025"),
        };

        public static Vector3F GetNextMove(Actor actor, Vector3F expectMove) {
            if (actor == null || expectMove == Vector3F.zero) {
                return Vector3F.zero;
            }

            List<int> uids = NavmeshUtils.RaycastInCircle(actor.Pos, DetectRadius);
            uids.Sort();

            List<Obstacle> obstacles = new List<Obstacle>();
            foreach (int uid in uids) {
                if (uid == actor.Uid) {
                    continue;
                }

                Actor obstacleActor = ActorUtils.GetActor(uid);
                MoveCom obstacleMoveCom = obstacleActor?.GetComponent<MoveCom>();
                Vector3F? nextMove = obstacleMoveCom?.NextExpectMove;
                if (nextMove.HasValue) {
                    obstacles.Add(new Obstacle(obstacleActor, nextMove.Value));
                }
            }

            foreach (Candidate candidate in Candidates) {
                Vector3F move = Rotate(expectMove, candidate.Cos, candidate.Sin);
                move = LimitInSurface(actor, move);
                if (move != Vector3F.zero && IsSafe(actor, expectMove, move, obstacles)) {
                    return move;
                }
            }

            return Vector3F.zero;
        }

        private static Vector3F Rotate(Vector3F move, FloatF cos, FloatF sin) {
            return new Vector3F(
                move.x * cos + move.z * sin,
                FloatF.zero,
                -move.x * sin + move.z * cos
            );
        }

        private static Vector3F LimitInSurface(Actor actor, Vector3F move) {
            Vector3F target = actor.Pos + move;
            if (NavmeshUtils.IsReachableByRadius(actor.Stats.Radius, target)) {
                Vector3F surfaceTarget = NavmeshUtils.RaycastInSurface(actor.Stats.Radius, actor.Pos, target);
                return surfaceTarget - actor.Pos;
            }
            return Vector3F.zero;
        }

        private static bool IsSafe(Actor actor, Vector3F expectMove, Vector3F candidateMove,
            List<Obstacle> obstacles) {
            foreach (Obstacle obstacle in obstacles) {
                Vector3F offset = actor.Pos - obstacle.Actor.Pos;
                FloatF radius = actor.Stats.Radius.Value + obstacle.Actor.Stats.Radius.Value;
                FloatF radius2 = radius * radius;
                FloatF currentDistance2 = Vector3F.Dot(offset, offset);

                // RVO 假设双方各承担一半速度修正。以双方原速度的平均值为锥顶，
                // 将当前候选速度映射成双方完成对称修正后的相对位移。
                Vector3F rvoApex = (expectMove + obstacle.ExpectMove) / FloatF.two;
                Vector3F reciprocalRelativeMove = (candidateMove - rvoApex) * FloatF.two;

                // 已经重叠时只允许能扩大间距的移动，避免双方原地锁死。
                if (currentDistance2 < radius2) {
                    Vector3F nextOffset = offset + reciprocalRelativeMove;
                    if (Vector3F.Dot(nextOffset, nextOffset) <= currentDistance2) {
                        return false;
                    }
                    continue;
                }

                Vector3F predictMove = reciprocalRelativeMove * PredictFrameCount;
                FloatF predictLength2 = Vector3F.Dot(predictMove, predictMove);
                if (predictLength2 <= FloatF.eps) {
                    continue;
                }

                FloatF time = FloatF.Clamp(
                    -Vector3F.Dot(offset, predictMove) / predictLength2,
                    FloatF.zero,
                    FloatF.one
                );
                Vector3F closestOffset = offset + predictMove * time;
                if (Vector3F.Dot(closestOffset, closestOffset) < radius2) {
                    return false;
                }
            }
            return true;
        }
    }
}
