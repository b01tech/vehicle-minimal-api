using Microsoft.EntityFrameworkCore;
using VehicleControl.API.Domain.Entities;

namespace VehicleControl.API.Infra.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public AppDbContext(DbContextOptions options) : base(options) { }

    protected AppDbContext() { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SetUserModel(modelBuilder);
        SetVehicleModel(modelBuilder);
    }

    private static void SetUserModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(u => u.Email) 
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(u => u.Role)
                .IsRequired();
        });


    }
    private static void SetVehicleModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.ToTable("vehicles");

            entity.HasKey(v => v.Id);

            entity.Property(v => v.LicencePlate)
                .IsRequired()
                .HasMaxLength(10);

            entity.HasIndex(v => v.LicencePlate) 
                .IsUnique();

            entity.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(v => v.Year)
                .IsRequired();
        });
    }

}
