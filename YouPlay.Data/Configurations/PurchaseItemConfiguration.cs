using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouPlay.Core.Entities;

namespace YouPlay.Data.Configurations
{
    public class PurchaseItemConfiguration : IEntityTypeConfiguration<PurchaseItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseItem> builder)
        {
            builder.HasKey(pi => new { pi.PurchaseId, pi.GameId });

            builder.HasOne(pi => pi.Game)
                .WithMany()
                .HasForeignKey(pi => pi.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
