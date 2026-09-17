using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestorantApp.Entity.Entities;

namespace RestorantApp.DataAccess.Configurations;

internal class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name).IsRequired()
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Price).IsRequired().HasPrecision(18, 2);

        builder.Property(m => m.Category).IsRequired();

        builder.HasIndex(m => m.Name).IsUnique();
    }
}
