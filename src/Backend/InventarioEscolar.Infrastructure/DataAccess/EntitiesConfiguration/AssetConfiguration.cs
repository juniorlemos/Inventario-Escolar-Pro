using InventarioEscolar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioEscolar.Infrastructure.DataAccess.EntitiesConfiguration
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets");

            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedOn).IsRequired();

            builder.Property(x => x.Active).IsRequired().HasDefaultValue(true);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200);
                               
            builder.Property(x => x.AcquisitionValue).HasPrecision(10, 2);
            builder.Property(x => x.SerieNumber).HasMaxLength(30);
            builder.Property(a => a.ConservationState).IsRequired().HasConversion<int>();
            
            builder.HasOne(a => a.Category)
               .WithMany(c => c.Assets) 
               .HasForeignKey(c => c.CategoryId)
               .IsRequired();


            builder.HasOne(a => a.RoomLocation)
               .WithMany(c => c.Assets)
               .HasForeignKey(r => r.RoomLocationId)
               .IsRequired(false);
        }
    }
}
