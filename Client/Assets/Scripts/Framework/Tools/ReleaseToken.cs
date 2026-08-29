using System;

namespace Framework {
    public class ReleaseToken {
        private bool isReleased = false;
        private Action releaseFunc;
        
        public ReleaseToken() {
            releaseFunc = null;
        }

        public ReleaseToken(Action releaseFunc) {
            this.releaseFunc = releaseFunc;
        }
        
        public void Release() {
            if (isReleased) {
                return;
            }

            releaseFunc?.Invoke();
            isReleased = true;
        }
    }
}