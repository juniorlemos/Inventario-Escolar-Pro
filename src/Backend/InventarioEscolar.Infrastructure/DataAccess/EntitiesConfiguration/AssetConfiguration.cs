using InventarioEscolar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioEscolar.Infrastructure.DataAccess.EntitiesConfiguration
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets", tb =>
            {
                tb.HasCheckConstraint("CK_Asset_Name_MinLength", "LEN(Name) >= 2");
            });

            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.Active).IsRequired().HasDefaultValue(true);
            builder.HasIndex(x => x.PatrimonyCode).IsUnique();
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200);              
            builder.Property(x => x.AcquisitionValue).HasPrecision(10, 2);
            builder.Property(x => x.SerieNumber).HasMaxLength(30);
            builder.Property(a => a.ConservationState).IsRequired().HasConversion<int>();
            
            builder.HasOne(a => a.Category)
               .WithMany(c => c.Assets) 
               .HasForeignKey(a => a.CategoryId)
               .IsRequired();

            builder.HasOne(a => a.RoomLocation)
               .WithMany(c => c.Assets)
               .HasForeignKey(a => a.RoomLocationId)
               .IsRequired(false);

            builder.HasOne(a => a.School)
               .WithMany(s => s.Assets)
               .HasForeignKey(a => a.SchoolId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}