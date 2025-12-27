namespace Network {
    public class MockLockStep : IFrameDriver {
        private int frame = 0;

        public int Frame => frame;

        public bool FrameReady() {
            frame++;
            return true;
        }
    }
}