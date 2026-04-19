public static class ArrayUtils {
    public static void InitArray<T>(ref T[][] array, int length1, int length2, T defaultValue = default) {
        array = new T[length1][];
        for (int i = 0; i < length1; i++) {
            array[i] = new T[length2];
            for (int j = 0; j < length2; j++) {
                array[i][j] = defaultValue;
            }
        }
    }
}
