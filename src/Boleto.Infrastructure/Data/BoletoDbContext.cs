using Boleto.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Boleto.Infrastructure.Data
{
    public class BoletoDbContext : DbContext
    {
        public BoletoDbContext(DbContextOptions<BoletoDbContext> options) : base(options) { }

        public DbSet<BoletoEntity> Boletos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BoletoEntity>(entity =>
            {
                entity.HasKey(b => b.BarCode);
                entity.Property(b => b.BarCode).IsRequired();
                entity.Property(b => b.OriginalAmount).HasColumnType("decimal(18,2)");
                entity.Property(b => b.Amount).HasColumnType("decimal(18,2)");
                entity.Property(b => b.DueDate).IsRequired();
                entity.Property(b => b.PaymentDate).IsRequired();
                entity.Property(b => b.InterestAmountCalculated).HasColumnType("decimal(18,2)");
                entity.Property(b => b.FineAmountCalculated).HasColumnType("decimal(18,2)");
            });
        }
    }
}