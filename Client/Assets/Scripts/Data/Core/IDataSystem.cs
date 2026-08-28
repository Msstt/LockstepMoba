namespace Data {
    public interface IDataSystem : ISystem, ICheckableSystem {
        public T Get<T>() where T : class, IData, new();
    }
}