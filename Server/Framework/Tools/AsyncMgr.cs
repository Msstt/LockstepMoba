namespace Framework {
    public class AsyncMgr : Singleton<AsyncMgr> {
        private Action onWaitFrameEnd;
        
        public void WaitFrameEnd(Action callback) {
            onWaitFrameEnd += callback;
        }

        public void Update() {
            WaitFrameEnd();
        }

        private void WaitFrameEnd() {
            Action temp = onWaitFrameEnd;
            onWaitFrameEnd = null;
            
            if (temp != null) {
                foreach (Action func in temp.GetInvocationList()) {
                    try {
                        func();
                    }
                    catch (Exception e) {
                        Console.WriteLine($"[AsyncMgr] callback error: {e.Message}");
                    }
                }
            }
        }
    }    
}
