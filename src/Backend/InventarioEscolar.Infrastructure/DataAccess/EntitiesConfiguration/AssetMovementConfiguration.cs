using InventarioEscolar.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.EntitiesConfiguration
{
    public class AssetMovementConfiguration : IEntityTypeConfiguration<AssetMovement>
    {
        public void Configure(EntityTypeBuilder<AssetMovement> builder)
        {
            builder.ToTable("AssetMovements");

            builder.HasKey(am => am.Id);
            builder.Property(am => am.Id).ValueGeneratedOnAdd();

            builder.Property(am => am.MovedAt).IsRequired();

            builder.HasOne(am => am.Asset)
                   .WithMany()
                   .HasForeignKey(am => am.AssetId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(am => am.FromRoom)
                   .WithMany()
                   .HasForeignKey(am => am.FromRoomId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(am => am.ToRoom)
                   .WithMany()
                   .HasForeignKey(am => am.ToRoomId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(am => am.Responsible).HasMaxLength(100);
        }
    }
}