using FinancialControl.Infra.Data;
using FinancialControl.Infra.Interfaces;

namespace FinancialControl.Infra.Repositories
{
    public class GeralRepo : IGeralRepo
    {
        private readonly DataContext _context;

        public GeralRepo(DataContext context)
        {
            _context = context;
        }

        public async void AddAsync<T>(T entity) where T : class
        {
            await _context.AddAsync(entity);
        }

        public void UpdateAsync<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void DeleteAsync<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
