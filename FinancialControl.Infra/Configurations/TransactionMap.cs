using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinancialControl.Domain.Entities;

namespace FinancialControl.Infra.Configurations
{
    public class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("Transaction");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).IsRequired().ValueGeneratedOnAdd().HasDefaultValueSql("uuid_generate_v4()");
            builder.Property(c => c.Type);        
            builder.Property(c => c.Date).HasMaxLength(20).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(128).IsRequired();
            builder.Property(c => c.Amount).HasColumnType("DECIMAL(18,2)").IsRequired();
        }
    }
}
