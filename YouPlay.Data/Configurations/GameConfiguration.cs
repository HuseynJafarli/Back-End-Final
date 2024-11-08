using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YouPlay.Core.Entities;

namespace YouPlay.Data.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.Description)
                .HasMaxLength(1000);

            builder.Property(g => g.CostPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(g => g.SalePrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(g => g.Discount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(g => g.ReleaseDate)
                .IsRequired();

            builder.HasMany(g => g.Comments)
                .WithOne(c => c.Game)
                .HasForeignKey(c => c.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(g => g.GameImages)
                .WithOne(gi => gi.Game)
                .HasForeignKey(gi => gi.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
