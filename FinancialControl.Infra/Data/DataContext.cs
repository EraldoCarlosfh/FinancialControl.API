using FinancialControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialControl.Infra.Configurations;

namespace FinancialControl.Infra.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new TransactionMap());
        }
    }
}
