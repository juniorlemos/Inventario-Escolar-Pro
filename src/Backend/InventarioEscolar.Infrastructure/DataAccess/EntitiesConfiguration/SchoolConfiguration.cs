using InventarioEscolar.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InventarioEscolar.Infrastructure.DataAccess.EntitiesConfiguration
{
    public class SchoolConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {

            builder.ToTable("Schools", tb =>
            {
                tb.HasCheckConstraint("CK_Schools_Name_MinLength", "LEN(Name) >= 2");
            });

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.Inep)
                   .HasMaxLength(20);

            builder.Property(s => s.Address)
                   .HasMaxLength(100);

            builder.Property(s => s.City)
                   .HasMaxLength(30);
            
            builder.HasIndex(s => s.Name)
                   .IsUnique();
            builder.HasIndex(s => s.Inep)
                   .IsUnique()
                   .HasFilter("[Inep] IS NOT NULL");

            builder.HasIndex(s => s.Address)
                   .IsUnique()
                   .HasFilter("[Address] IS NOT NULL");

            builder.HasMany(s => s.Users)
                   .WithOne(u => u.School)
                   .HasForeignKey(u => u.SchoolId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.RoomLocations)
                   .WithOne(r => r.School)
                   .HasForeignKey(r => r.SchoolId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Assets)
                   .WithOne(a => a.School)
                   .HasForeignKey(a => a.SchoolId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
