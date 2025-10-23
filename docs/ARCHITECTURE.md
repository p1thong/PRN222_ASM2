# Architecture Documentation

## 📐 System Architecture

The Car Sales Management System follows a **3-tier layered architecture** pattern with clear separation of concerns.

## 🏗️ High-Level Architecture

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│                    (ASM1.WebMVC)                        │
│  Controllers • Views • ViewModels • Client Assets       │
└─────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────┐
│                   Business Logic Layer                   │
│                    (ASM1.Service)                       │
│      Services • Business Rules • Validations            │
└─────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────┐
│                   Data Access Layer                      │
│                  (ASM1.Repository)                      │
│   Repositories • Unit of Work • EF Core • Models        │
└─────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────┐
│                       Database                           │
│                     SQL Server                           │
└─────────────────────────────────────────────────────────┘
```

## 📦 Layer Details

### 1. Presentation Layer (ASM1.WebMVC)

**Responsibilities:**
- Handle HTTP requests and responses
- Render views using Razor
- Validate user input
- Route requests to appropriate controllers
- Manage session and authentication state

**Components:**
- **Controllers**: Handle incoming requests, call services, return views
- **Views**: Razor templates for UI rendering
- **ViewModels**: Data transfer objects for views
- **Extensions**: Helper methods for controllers
- **wwwroot**: Static files (CSS, JS, images)

**Key Patterns:**
- MVC (Model-View-Controller)
- AutoMapper for object mapping
- Dependency Injection

### 2. Business Logic Layer (ASM1.Service)

**Responsibilities:**
- Implement business rules and validations
- Coordinate multiple repository operations
- Handle business exceptions
- Perform data transformations
- Apply business logic workflows

**Components:**
- **Services**: Business logic implementations
- **Interfaces**: Service contracts
- **Exceptions**: Custom business exceptions
- **Utilities**: Helper classes (e.g., PromotionCodeGenerator)

**Key Services:**
- `AuthService`: Authentication and authorization
- `CustomerService`: Customer management
- `VehicleService`: Vehicle operations
- `OrderService`: Order processing
- `PaymentService`: Payment handling
- `PromotionService`: Promotion management
- `SalesContractService`: Contract operations
- `QuotationService`: Quote generation

**Key Patterns:**
- Service Layer Pattern
- Dependency Injection
- Strategy Pattern (for promotions)
- Exception Handling

### 3. Data Access Layer (ASM1.Repository)

**Responsibilities:**
- Database operations (CRUD)
- Query optimization
- Transaction management
- Data persistence
- Database migrations

**Components:**
- **Models**: Entity classes (POCOs)
- **Repositories**: Data access implementations
- **Interfaces**: Repository contracts
- **DbContext**: Entity Framework Core context
- **UnitOfWork**: Transaction coordinator
- **Utilities**: Helper classes (e.g., IdGenerator)

**Key Repositories:**
- `CustomerRepository`
- `VehicleRepository`
- `VehicleModelRepository`
- `VehicleVariantRepository`
- `OrderRepository`
- `PaymentRepository`
- `SalesContractRepository`
- `QuotationRepository`
- `PromotionRepository`
- `FeedbackRepository`

**Key Patterns:**
- Repository Pattern
- Unit of Work Pattern
- Generic Repository
- Entity Framework Core

## 🔄 Request Flow

### Typical Request Flow Example: Create Order

```
1. User submits order form
   ↓
2. OrderController receives POST request
   ↓
3. Controller validates ViewModel
   ↓
4. Controller calls OrderService.CreateOrderAsync()
   ↓
5. OrderService validates business rules
   ↓
6. OrderService calls multiple repositories via UnitOfWork
   - Check vehicle availability
   - Validate customer
   - Calculate pricing with promotions
   - Create order record
   - Create payment record
   ↓
7. UnitOfWork commits transaction
   ↓
8. OrderService returns result to Controller
   ↓
9. Controller redirects to success page
   ↓
10. View is rendered and sent to user
```

## 🔐 Security Architecture

### Authentication & Authorization
- Cookie-based authentication
- Role-based access control (RBAC)
- Session management
- Password hashing (recommended: ASP.NET Identity)

### Data Protection
- SQL Injection prevention (EF Core parameterization)
- XSS protection (Razor encoding)
- CSRF tokens
- Input validation

## 💾 Database Design

### Key Entities

**Core Entities:**
- `User`: System users with roles
- `Customer`: Customer information
- `Manufacturer`: Vehicle manufacturers
- `Dealer`: Dealerships and locations

**Vehicle Entities:**
- `VehicleModel`: Vehicle models
- `VehicleVariant`: Specific variants of models

**Transaction Entities:**
- `Order`: Customer orders
- `Payment`: Payment records
- `SalesContract`: Sales agreements
- `Quotation`: Price quotes
- `Promotion`: Promotional offers

**Supporting Entities:**
- `DealerContract`: Dealer agreements
- `Feedback`: Customer feedback
- `TestDrive`: Test drive appointments

### Entity Relationships

```
Manufacturer 1──N VehicleModel 1──N VehicleVariant
     │
     └──── 1──N Dealer 1──N DealerContract

Customer 1──N Order ──N VehicleVariant
    │         │
    │         └──1 Payment
    │         └──1 SalesContract
    │         └──1 Quotation
    │
    └──N TestDrive ──1 VehicleVariant
    └──N Feedback
```

## 🔌 Dependency Injection

### Service Registration (Program.cs)

```csharp
// Repository layer
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Service layer
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
// ... other services

// DbContext
builder.Services.AddDbContext<CarSalesDbContext>(options =>
    options.UseSqlServer(connectionString));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
```

## 📊 Design Patterns Used

### Creational Patterns
- **Dependency Injection**: Throughout the application
- **Factory Pattern**: IdGenerator, PromotionCodeGenerator

### Structural Patterns
- **Repository Pattern**: Data access abstraction
- **Adapter Pattern**: AutoMapper for object mapping

### Behavioral Patterns
- **Strategy Pattern**: Promotion rule validation
- **Unit of Work Pattern**: Transaction management
- **Chain of Responsibility**: Exception handling

## 🔧 Configuration Management

### Configuration Sources
1. `appsettings.json` - Base configuration
2. `appsettings.Development.json` - Development overrides
3. Environment variables - Production secrets
4. User secrets - Local development

### Key Configuration Sections
- ConnectionStrings
- Logging
- Authentication
- Application settings

## 🚀 Scalability Considerations

### Current Architecture Supports:
- **Horizontal Scaling**: Stateless web tier
- **Caching**: Ready for Redis/Memory cache
- **Load Balancing**: Multiple web server instances
- **Database Scaling**: Connection pooling, read replicas

### Future Enhancements:
- Message queues (e.g., RabbitMQ) for async operations
- Microservices decomposition
- Event-driven architecture
- CQRS pattern for read/write separation

## 📈 Performance Optimization

### Database Level
- Proper indexing on foreign keys and search fields
- Query optimization with LINQ
- Eager/lazy loading strategies
- Connection pooling

### Application Level
- Output caching for static content
- ViewComponent caching
- Async/await for I/O operations
- AutoMapper projection

### Client Level
- Bundling and minification
- Browser caching
- CDN for static assets
- Image optimization

## 🧪 Testing Strategy

### Unit Tests
- Service layer business logic
- Repository layer queries
- Utility functions

### Integration Tests
- Controller actions
- Repository with in-memory database
- End-to-end workflows

### UI Tests (Optional)
- Selenium for critical paths
- User acceptance testing

## 📝 Code Organization

### Naming Conventions
- **Interfaces**: `I{Name}` (e.g., `ICustomerService`)
- **Implementations**: `{Name}` (e.g., `CustomerService`)
- **ViewModels**: `{Entity}ViewModel`
- **Controllers**: `{Entity}Controller`

### Folder Structure
- Group by feature/entity
- Separate interfaces from implementations
- Utilities in dedicated folders

## 🔍 Error Handling

### Exception Hierarchy
```
Exception
  └── BusinessException (custom)
       ├── NotFoundException
       ├── ValidationException
       ├── DuplicateException
       └── UnauthorizedException
```

### Error Flow
1. Repository throws data exceptions
2. Service catches and throws business exceptions
3. Controller catches and returns appropriate HTTP responses
4. Global exception handler for unhandled exceptions

## 🌐 API Design (Future)

### RESTful Principles
- Resource-based URLs
- HTTP verbs (GET, POST, PUT, DELETE)
- Status codes (200, 201, 400, 404, 500)
- JSON responses

### Example Endpoints
```
GET    /api/vehicles          - List vehicles
GET    /api/vehicles/{id}     - Get vehicle
POST   /api/vehicles          - Create vehicle
PUT    /api/vehicles/{id}     - Update vehicle
DELETE /api/vehicles/{id}     - Delete vehicle
```

## 📚 References

- [ASP.NET Core MVC Documentation](https://docs.microsoft.com/en-us/aspnet/core/mvc/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)

---

**Last Updated**: October 2024
**Author**: Development Team

