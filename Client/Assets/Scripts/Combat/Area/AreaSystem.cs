using System.Collections.Generic;
using Framework;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;
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

        public void Init() {
            TransRoot = new GameObject("[Area]").transform;
        }
        
        public void Update() {
            foreach (var (uid, areaInfo) in areaInfos) {
                areaInfo.area.RenderUpdate();
            }
        }


        public void FrameUpdate(int frame) {
            foreach (var (uid, areaInfo) in areaInfos) {
                areaInfo.area.Update();
                if (areaInfo.endFrame <= frame) {
                    areaInfo.area.Dispose();
                    areaInfos.Remove(uid);
                }
            }
        }

        public int CreateArea(int areaId, int actorId, int level, Vector3F position, Vector3F direction) {
             Area area = new Area(areaId, actorId, level, position, direction);
             int uid = ++maxId;
             areaInfos[uid] = new AreaInfo() {
                 uid = uid,
                 area = area,
                 endFrame = TimeUtils.GetFrame(Config.Area[areaId].Time),
             };
             return uid;
        }

        public void DestroyArea(int uid) {
            if (areaInfos.ContainsKey(uid)) {
                areaInfos[uid].area.Dispose();
                areaInfos.Remove(uid);
            }
        }
    }
}