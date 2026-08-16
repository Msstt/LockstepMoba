using Combat.Actor;
using Combat.Skill;

namespace Combat.BehaviourMachine {
    public class MinionWaveBehaviour : ActorBehaviour {
        public override SkillType SkillType => SkillType.Move;
        private MinionWave wave;
        private int index;
        private bool isMoving = false;

        private static FloatF eps = FloatF.two;
        
        public MinionWaveBehaviour(Machine machine, int waveIndex) : base(machine) {
            var waves = Actor.Camp == CampType.Blue ? Config.Map.blueMinionWavePos : Config.Map.redMinionWavePos;
            if (waveIndex >= waves.Count) {
                throw new CombatException($"MinionWaveBehaviour: waveIndex {waveIndex} is out of range.");
            }
            wave = waves[waveIndex];
        }
        
        public override bool Evaluate() {
            if (index >= wave.Pos.Count) {
                Log.Warning($"MinionWaveBehaviour: index {index} is out of range.");
            }
            return true;
        }
        
        public override void Execute(int frame) {
            if (index >= wave.Pos.Count || isMoving) {
                return;
            }

            UpdateIndex();
            
            if (index >= wave.Pos.Count) {
                return;
            }

            isMoving = true;
            var targetPos = wave.Pos[index];
            Actor.GetComponent<MoveCom>().MoveToPosByPath(targetPos.position, StopMove, StopMove);
            Actor.GetComponent<AnimCom>()?.PlayAnim("Move");
        }
        
        public override void OnAbort() {
            Actor.GetComponent<MoveCom>().ForceFail();
        }
        
        private void StopMove() {
            isMoving = false;
            Actor.GetComponent<AnimCom>()?.PlayAnim("Idle");
        }

        private void UpdateIndex() {
            if (Vector3F.Distance(Actor.Pos, wave.Pos[index].position) < eps) {
                index++;
                return;
            }

            if (index > 0) {
                Vector3F pos0 = wave.Pos[index - 1].position;
                Vector3F pos1 = wave.Pos[index].position;
                if (Vector3F.Dot((Actor.Pos - pos1), (pos0 - pos1)) < FloatF.zero) {
                    index++;
                    return;
                }
            }
        }
    }
}