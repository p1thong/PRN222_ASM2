# Car Sales Management System (PRN222 Assignment 2)

## 📋 Project Overview

A comprehensive Car Sales Management System built with ASP.NET Core MVC, designed to manage vehicle sales, dealer contracts, customer relationships, and promotions.

## 🏗️ Architecture

This solution follows a clean architecture pattern with three main layers:

### Projects Structure

```
PRN222_ASM2/
├── ASM1.Repository/     # Data Access Layer
├── ASM1.Service/        # Business Logic Layer
└── ASM1.WebMVC/         # Presentation Layer (MVC)
```

### ASM1.Repository
- **Data Access Layer** using Entity Framework Core
- Repository pattern implementation
- Unit of Work pattern
- Database context and seed data

### ASM1.Service
- **Business Logic Layer**
- Service interfaces and implementations
- Business rule validations
- Exception handling

### ASM1.WebMVC
- **Presentation Layer** using ASP.NET Core MVC
- Controllers and Views
- ViewModels and mapping profiles
- Client-side assets (CSS, JS)

## 🚀 Features

### Core Modules
- **User Authentication & Authorization**
- **Customer Management**
- **Vehicle Management** (Models, Variants)
- **Manufacturer Management**
- **Dealer Contract Management**
- **Sales Contract Management**
- **Order Processing**
- **Payment Processing**
- **Quotation System**
- **Promotion Management**
- **Feedback System**
- **Test Drive Scheduling**

## 🛠️ Technologies

- **Framework**: ASP.NET Core MVC
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **Mapping**: AutoMapper
- **Architecture**: Repository Pattern, Unit of Work Pattern
- **Frontend**: Razor Views, Bootstrap, JavaScript

## 📦 Database Models

- Customer
- User
- Dealer & DealerContract
- Manufacturer
- VehicleModel & VehicleVariant
- Order
- Payment
- SalesContract
- Quotation
- Promotion
- Feedback
- TestDrive

## 🔧 Setup Instructions

### Prerequisites
- .NET 6.0 SDK or later
- SQL Server
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
```bash
git clone <repository-url>
cd PRN222_ASM2
```

2. Restore dependencies
```bash
dotnet restore
```

3. Update database connection string in `ASM1.WebMVC/appsettings.json`

4. Apply migrations
```bash
dotnet ef database update --project ASM1.Repository --startup-project ASM1.WebMVC
```

5. Run the application
```bash
dotnet run --project ASM1.WebMVC
```

## 👥 Team Information

- **Course**: PRN222 - Advanced Cross-platform Application Programming with .NET
- **Assignment**: ASM2
- **Institution**: FPT University

## 📄 License

This project is for educational purposes only.

## 📞 Support

For questions or issues, please contact your instructor or teaching assistant.

---

**Note**: This is a student project for learning purposes.

