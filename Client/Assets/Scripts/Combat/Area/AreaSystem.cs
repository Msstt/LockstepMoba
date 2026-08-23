using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace Combat.Area {
    public class AreaSystem : IAreaSystem {
        public Transform TransRoot { private set; get; }
        
        private class AreaInfo {
            public int uid;
            public Area area;
            public int endFrame;
        }

        public int maxId = 0;
        
        private SafeDictionary<int, AreaInfo> areaInfos = new SafeDictionary<int, AreaInfo>();
        private List<int> toDestroy = new List<int>();

        public void Init() {
            TransRoot = new GameObject("[Area]").transform;
        }
        
        public void Update() {
            foreach (var (uid, areaInfo) in areaInfos) {
                areaInfo.area.RenderUpdate();
            }
        }


        public void FrameUpdate(int frame) {
            DestroyArea();
            foreach (var (uid, areaInfo) in areaInfos) {
                areaInfo.area.Update();
                if (areaInfo.endFrame <= frame) {
                    areaInfo.area.Dispose();
                    areaInfos.Remove(uid);
                }
            }
            DestroyArea();
        }

        public int CreateArea(int areaId, int actorId, int level, Vector3F position, Vector3F direction, int? targetUid) {
             int uid = ++maxId;
             Area area = new Area(areaId, uid, actorId, level, position, direction, targetUid);
             areaInfos[uid] = new AreaInfo() {
                 uid = uid,
                 area = area,
                 endFrame = TimeUtils.GetFrame(Config.Area[areaId].Time),
             };
             return uid;
        }

        public void DestroyArea(int uid) {
            toDestroy.Add(uid);
        }

        private void DestroyArea() {
            foreach (var uid in toDestroy) {
                if (areaInfos.ContainsKey(uid)) {
                    areaInfos[uid].area.Dispose();
                    areaInfos.Remove(uid);
                }
            }
            toDestroy.Clear();
        }
    }
}