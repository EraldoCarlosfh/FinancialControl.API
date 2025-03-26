using FinancialControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using FinancialControl.Infra.Configurations;
using FinancialControl.Domain.Enums;

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
            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    Id = Guid.Parse("864e23f5-4935-4539-9ff3-b9814df05803"),
                    Type = TransactionType.Despesa,
                    Date = DateTime.Parse("2022-08-29"),
                    Description = "Cartão de Crédito",
                    Amount = 825.82M
                },
                new Transaction
                {
                    Id = Guid.Parse("f84ece94-ebb3-473e-8a01-9dcc5c230cf5"),
                    Type = TransactionType.Despesa,
                    Date = DateTime.Parse("2022-08-29"),
                    Description = "Curso C#",
                    Amount = 200.00M
                },
                new Transaction
                {
                    Id = Guid.Parse("030905a5-d250-49df-8f0e-fde17201725c"),
                    Type = TransactionType.Receita,
                    Date = DateTime.Parse("2022-08-31"),
                    Description = "Salário",
                    Amount = 7000.00M
                },
                new Transaction
                {
                    Id = Guid.Parse("04c66057-f976-4f64-b92a-173058da5ca6"),
                    Type = TransactionType.Despesa,
                    Date = DateTime.Parse("2022-09-01"),
                    Description = "Mercado",
                    Amount = 3000.00M
                },
                new Transaction
                {
                    Id = Guid.Parse("04c66057-f976-4f64-b92a-a5ca6173058d"),
                    Type = TransactionType.Despesa,
                    Date = DateTime.Parse("2022-09-01"),
                    Description = "Farmácia",
                    Amount = 300.00M
                },
                new Transaction
                {
                    Id = Guid.Parse("2872bc54-a9af-4677-87f2-4687de59b423"),
                    Type = TransactionType.Despesa,
                    Date = DateTime.Parse("2022-09-01"),
                    Description = "Combustível",
                    Amount = 800.25M
                },
                new Transaction
                {
                    Id = Guid.Parse("14b74b57-f445-48a2-913a-729228588169"),
                    Type = TransactionType.Despesa,
                    Date = DateTime.Parse("2022-09-15"),
                    Description = "Financiamento Carro",
                    Amount = 900.00M
                },
                new Transaction
                {
                    Id = Guid.Parse("e547968c-267d-4571-a593-09f39f98f7e6"),
                    Type = TransactionType.Despesa,
                    Date = DateTime.Parse("2022-09-22"),
                    Description = "Financiamento Casa",
                    Amount = 1200.00M
                },
                new Transaction
                {
                    Id = Guid.Parse("3fe223d4-edc9-43bb-8a38-d4cd6d65b745"),
                    Type = TransactionType.Receita,
                    Date = DateTime.Parse("2022-09-25"),
                    Description = "Freelance Projeto XPTO",
                    Amount = 2500.00M
                }
            );
        }
    }
}