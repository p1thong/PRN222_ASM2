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
            },
            new Manufacturer
            {
                ManufacturerId = 2,
                Name = "Tesla",
                Country = "USA",
                Address = "3500 Deer Creek Road, Palo Alto, CA, USA"
            },
            new Manufacturer
            {
                ManufacturerId = 3,
                Name = "VinFast",
                Country = "Vietnam",
                Address = "Dinh Vu - Cat Hai Economic Zone, Hai Phong, Vietnam"
            },
            new Manufacturer
            {
                ManufacturerId = 4,
                Name = "BYD",
                Country = "China",
                Address = "No. 3009, BYD Road, Pingshan, Shenzhen, China"
            },
            new Manufacturer
            {
                ManufacturerId = 5,
                Name = "NIO",
                Country = "China",
                Address = "No. 56, AnTuo Road, Anting Town, Jiading District, Shanghai, China"
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

        // Seed Vehicle Models
        modelBuilder.Entity<VehicleModel>().HasData(
            // Tesla
            new VehicleModel { VehicleModelId = 1, ManufacturerId = 2, Name = "Model S", Category = "Sedan" },
            new VehicleModel { VehicleModelId = 2, ManufacturerId = 2, Name = "Model 3", Category = "Sedan" },
            // VinFast
            new VehicleModel { VehicleModelId = 3, ManufacturerId = 3, Name = "VF e34", Category = "SUV" },
            new VehicleModel { VehicleModelId = 4, ManufacturerId = 3, Name = "VF 8", Category = "SUV" },
            // BYD
            new VehicleModel { VehicleModelId = 5, ManufacturerId = 4, Name = "Han EV", Category = "Sedan" },
            new VehicleModel { VehicleModelId = 6, ManufacturerId = 4, Name = "Atto 3", Category = "SUV" },
            // NIO
            new VehicleModel { VehicleModelId = 7, ManufacturerId = 5, Name = "ES6", Category = "SUV" },
            new VehicleModel { VehicleModelId = 8, ManufacturerId = 5, Name = "ET7", Category = "Sedan" }
        );

        // Seed Vehicle Variants
        modelBuilder.Entity<VehicleVariant>().HasData(
            // Tesla Model S
            new VehicleVariant { VariantId = 1, VehicleModelId = 1, Version = "Long Range", Color = "White", ProductYear = 2024, Price = 90000 },
            new VehicleVariant { VariantId = 2, VehicleModelId = 1, Version = "Plaid", Color = "Black", ProductYear = 2025, Price = 120000 },
            // Tesla Model 3
            new VehicleVariant { VariantId = 3, VehicleModelId = 2, Version = "Standard", Color = "Blue", ProductYear = 2024, Price = 45000 },
            new VehicleVariant { VariantId = 4, VehicleModelId = 2, Version = "Performance", Color = "Red", ProductYear = 2025, Price = 60000 },
            // VinFast VF e34
            new VehicleVariant { VariantId = 5, VehicleModelId = 3, Version = "Base", Color = "White", ProductYear = 2024, Price = 35000 },
            // VinFast VF 8
            new VehicleVariant { VariantId = 6, VehicleModelId = 4, Version = "Plus", Color = "Gray", ProductYear = 2025, Price = 55000 },
            // BYD Han EV
            new VehicleVariant { VariantId = 7, VehicleModelId = 5, Version = "Premium", Color = "Black", ProductYear = 2024, Price = 70000 },
            // BYD Atto 3
            new VehicleVariant { VariantId = 8, VehicleModelId = 6, Version = "Comfort", Color = "Blue", ProductYear = 2025, Price = 40000 },
            // NIO ES6
            new VehicleVariant { VariantId = 9, VehicleModelId = 7, Version = "Standard", Color = "White", ProductYear = 2024, Price = 65000 },
            // NIO ET7
            new VehicleVariant { VariantId = 10, VehicleModelId = 8, Version = "Performance", Color = "Silver", ProductYear = 2025, Price = 90000 }
        );
    }
    }
