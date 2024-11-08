using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YouPlay.Data.Configurations
{
    public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Fullname)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Country)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.City)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.EmailAddress)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Address)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.ZipCode)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.Phone)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(p => p.TotalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(p => p.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.PurchaseItems)
                .WithOne(pi => pi.Purchase)
                .HasForeignKey(pi => pi.PurchaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
