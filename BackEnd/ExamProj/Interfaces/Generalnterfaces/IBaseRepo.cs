namespace WebApplication1.Interfaces
{
    public interface IBaseRepo<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T?> GetByID(int id);
        Task<T?> Update(int id, T entity);
        Task<bool> Delete(int id);
        Task<T?> Add(T entity);
    }
}
