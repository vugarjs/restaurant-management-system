using Microsoft.EntityFrameworkCore;
using RestorantApp.Entity.Entities;

namespace RestorantApp.DataAccess.Context;

public class RestorantContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var stringConnection = "Data Source=localhost\\SQLEXPRESS;Database=RestorantDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";
        optionsBuilder.UseSqlServer(stringConnection);
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestorantContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
