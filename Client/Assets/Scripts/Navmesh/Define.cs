namespace Navmesh {
    public static class FindPathConfig {
        public enum WType {
            centroidDis,
            edgeMidDis,
        }
        
        public static WType wType = WType.edgeMidDis;

        public static int FindPathMaxIterationCount = 1000;
        public static int FindPathMaxQueryPerFrame = 10;
    }
}