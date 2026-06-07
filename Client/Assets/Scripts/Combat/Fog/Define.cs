namespace Combat.Fog {
    public static class FogConfig {
        public static int VisionCellCount = 128;
    }

    public enum VisionType {
        None = -1,
        Global = 0, // 全局视野
        Self = 1, // 我方视野
        Limit = 2, // 受限视野（致盲...)
    }
}