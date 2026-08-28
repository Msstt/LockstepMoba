using UnityEngine;

namespace Combat.Area {
    public interface IAreaSystem : ISystem, IInitSystem, IUpdateSystem, IFrameUpdateSystem, ICheckableSystem {
        public Transform TransRoot { get; }

        public int CreateArea(int areaId, int actorId, int level, Vector3F position, Vector3F direction, int? targetUid);
        public void DestroyArea(int uid);
    }
}