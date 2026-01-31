// 角色上半身下半身占用、请求

using System;
using System.Collections.Generic;

namespace Combat.Actor {
    public class SlotCom : Com {
        [Flags]
        public enum Slot {
            None = 0,
            Upper = 1 << 1,
            Lower = 1 << 2,
        }

        private class RequestInfo {
            public int slot;
            public int frame;
            public Action<Action> callback;
            public Action failCallback;
        }
        
        private int curOccupancy = 0;

        private long requestId = 0;
        private readonly List<long> requestsList = new List<long>();
        private readonly Dictionary<long, RequestInfo> requests = new Dictionary<long, RequestInfo>();
        List<long> delRequestIds = new List<long>();

        public override void Update(int frame) {
            int requestSlot = 0;
            foreach (var id in requestsList) {
                RequestInfo info = requests[id];
                if (info.frame <= frame) {
                    info.failCallback?.Invoke();
                    delRequestIds.Add(id);
                    continue;
                }
                if ((requestSlot & info.slot) == 0 && !IsOccupy(info.slot)) {
                    Occupy(info.slot);
                    info.callback?.Invoke(GetReleaseFunc(info.slot));
                    delRequestIds.Add(id);
                } else {
                    requestSlot |= info.slot;
                }
            }
            foreach (var id in delRequestIds) {
                requestsList.Remove(id);
                requests.Remove(id);
            }
            delRequestIds.Clear();
        }

        private void Occupy(int slot) {
            curOccupancy |= slot;
        }
        
        private void Release(int slot) {
            curOccupancy &= ~slot;
        }

        private bool IsOccupy(int slot) {
            return (curOccupancy & slot) != 0;
        }
        
        private Action GetReleaseFunc(int slot) {
            bool isReleased = false;
            return () => {
                if (isReleased) {
                    return;
                }
                isReleased = true;
                Release(slot);
                Update(GameMgr.Instance.Frame);
            };
        }

        #region 接口

        // 返回 requestId，取消请求使用 Cancel(requestId)
        // callback(ReleaseFunc)
        public long RequestInTime(int slot, FloatF time, Action<Action> callback, Action failCallback = null) {
            long id = ++requestId;
            requests[id] = new RequestInfo {
                slot = slot,
                frame = TimeUtils.GetFrame(time),
                callback = callback,
                failCallback = failCallback,
            };
            requestsList.Add(id);
            return id;
        }
        
        public void Cancel(long id) {
            if (requests.ContainsKey(id)) {
                requestsList.Remove(id);
                requests.Remove(id);
            }
        }

        #endregion
    }
}