using InventarioEscolar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioEscolar.Infrastructure.DataAccess.EntitiesConfiguration
{
    public class RoomLocationConfiguration : IEntityTypeConfiguration<RoomLocation>
    {
        public void Configure(EntityTypeBuilder<RoomLocation> builder)
        {
            builder.ToTable("RoomLocations", tb =>
            {
                tb.HasCheckConstraint("CK_RoomLocation_Name_MinLength", "LEN(Name) >= 2");
            });


            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedOn).IsRequired();

            builder.Property(x => x.Active).IsRequired().HasDefaultValue(true);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Description).HasMaxLength(200);
            builder.Property(x => x.Building).HasMaxLength(50);

            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasMany(c => c.Assets)
             .WithOne(a => a.RoomLocation)
             .HasForeignKey(a => a.RoomLocationId);
        }
    }
}
