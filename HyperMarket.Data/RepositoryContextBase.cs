namespace HyperMarket.Data {
    public abstract class RepositoryContextBase {
        public abstract IRepository<T> GetRepository<T>() where T : class;
        public virtual void InitializeDatabase() { }
        public virtual void DeleteDatabase() { }
    }
}