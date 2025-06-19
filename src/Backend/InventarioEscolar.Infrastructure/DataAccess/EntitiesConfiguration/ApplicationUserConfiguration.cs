using InventarioEscolar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioEscolar.Infrastructure.DataAccess.EntitiesConfiguration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
      
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(u => u.School)
                   .WithMany(s => s.Users) 
                   .HasForeignKey(u => u.SchoolId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
