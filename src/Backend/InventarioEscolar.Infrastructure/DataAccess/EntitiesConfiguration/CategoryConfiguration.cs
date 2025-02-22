﻿using InventarioEscolar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventarioEscolar.Infrastructure.DataAccess.EntitiesConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(x => x.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedOn).IsRequired();

            builder.Property(x => x.Active).IsRequired().HasDefaultValue(true);

            builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(200);

            builder.HasMany(c => c.Assets)
             .WithOne(a => a.Category)
             .HasForeignKey(a => a.CategoryId)
             .IsRequired();
        }   
    }
}
