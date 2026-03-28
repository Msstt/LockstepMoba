namespace Data {
    public interface IDataSystem : ISystem {
        public T Get<T>() where T : class, IData, new();
    }
}