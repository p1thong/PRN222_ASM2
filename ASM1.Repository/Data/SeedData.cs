using ASM1.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Seed Manufacturers
        modelBuilder.Entity<Manufacturer>().HasData(
            new Manufacturer
            {
                ManufacturerId = 1,
                Name = "Toyota Motors",
                Country = "Japan",
                Address = "Toyota City, Aichi, Japan"
            }
        );

        // Seed Dealers
        modelBuilder.Entity<Dealer>().HasData(
            new Dealer
            {
                DealerId = 1,
                FullName = "Downtown Toyota Dealer",
                Email = "dealer@toyota-downtown.com",
                Password = "dealer123", // In production, this should be hashed
                Phone = "+1-555-0123",
                TransactionId = 1001
            }
        );

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            // Admin User
            new User
            {
                UserId = 1,
                FullName = "System Administrator",
                Email = "admin@carsales.com",
                Phone = "+1-555-0001",
                Password = "admin123", // In production, this should be hashed
                Role = "Admin",
                DealerId = null,
                ManufacturerId = null
            },
            // Dealer Staff User
            new User
            {
                UserId = 2,
                FullName = "John Smith",
                Email = "john.smith@toyota-downtown.com",
                Phone = "+1-555-0002",
                Password = "staff123", // In production, this should be hashed
                Role = "DealerStaff",
                DealerId = 1,
                ManufacturerId = null
            },
            // Dealer Manager User
            new User
            {
                UserId = 3,
                FullName = "Sarah Johnson",
                Email = "sarah.johnson@toyota-downtown.com",
                Phone = "+1-555-0003",
                Password = "manager123", // In production, this should be hashed
                Role = "DealerManager",
                DealerId = 1,
                ManufacturerId = null
            }
        );
    }
}