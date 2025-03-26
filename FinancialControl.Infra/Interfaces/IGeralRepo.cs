namespace FinancialControl.Infra.Interfaces
{
    public interface IGeralRepo
    {
        void AddAsync<T>(T entity) where T : class;
        void UpdateAsync<T>(T entity) where T : class;
        void DeleteAsync<T>(T entity) where T : class;
        Task<bool> SaveChangesAsync();
    }
}
