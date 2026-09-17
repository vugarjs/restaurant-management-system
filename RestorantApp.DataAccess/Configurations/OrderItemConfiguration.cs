using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestorantApp.Entity.Entities;

namespace RestorantApp.DataAccess.Configurations;

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(oi => oi.Count)
               .IsRequired();

        builder.HasOne(oi => oi.Order)
               .WithMany(o => o.OrderItems)
               .HasForeignKey(oi => oi.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(oi => oi.MenuItem)
               .WithMany()
               .HasForeignKey(oi => oi.MenuItemId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasKey(oi => new { oi.OrderId, oi.MenuItemId });
    }
}
