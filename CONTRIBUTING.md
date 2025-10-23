# Contributing Guidelines

## 🤝 How to Contribute

Thank you for considering contributing to the Car Sales Management System project!

## 📋 Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Coding Standards](#coding-standards)
- [Commit Guidelines](#commit-guidelines)
- [Pull Request Process](#pull-request-process)

## 📜 Code of Conduct

- Be respectful and inclusive
- Follow academic integrity guidelines
- Help teammates learn and grow
- Ask questions when unsure

## 🚀 Getting Started

### Prerequisites

Before you begin, ensure you have:
- .NET 6.0 SDK or later installed
- SQL Server installed and running
- Visual Studio 2022 or VS Code with C# extension
- Git for version control

### Setting Up Development Environment

1. Fork the repository (if working in a team)
2. Clone your fork locally
3. Create a new branch for your feature
4. Make your changes
5. Test thoroughly
6. Submit a pull request

## 🔄 Development Workflow

### Branch Naming Convention

- `feature/` - New features (e.g., `feature/add-inventory-module`)
- `bugfix/` - Bug fixes (e.g., `bugfix/fix-login-error`)
- `hotfix/` - Critical fixes (e.g., `hotfix/payment-calculation`)
- `docs/` - Documentation updates (e.g., `docs/update-readme`)

### Example Workflow

```bash
# Create a new feature branch
git checkout -b feature/your-feature-name

# Make your changes
# ... code, code, code ...

# Add and commit your changes
git add .
git commit -m "feat: add new feature description"

# Push to your branch
git push origin feature/your-feature-name

# Create a pull request
```

## 📝 Coding Standards

### C# Coding Conventions

- Follow Microsoft's C# coding conventions
- Use meaningful variable and method names
- Add XML comments for public methods and classes
- Keep methods focused and concise (Single Responsibility Principle)

### Example:

```csharp
/// <summary>
/// Retrieves a customer by their unique identifier
/// </summary>
/// <param name="customerId">The customer's ID</param>
/// <returns>Customer object if found, null otherwise</returns>
public async Task<Customer> GetCustomerByIdAsync(string customerId)
{
    return await _context.Customers
        .FirstOrDefaultAsync(c => c.CustomerId == customerId);
}
```

### Project Structure Rules

- **Repository Layer**: Only data access logic, no business rules
- **Service Layer**: Business logic and validation
- **Web/MVC Layer**: Presentation logic only
- Keep layers separated and follow dependency injection

### Naming Conventions

- **Classes**: PascalCase (e.g., `CustomerService`)
- **Methods**: PascalCase (e.g., `GetAllCustomers`)
- **Variables**: camelCase (e.g., `customerId`)
- **Private fields**: _camelCase (e.g., `_context`)
- **Constants**: UPPER_CASE (e.g., `MAX_RETRY_COUNT`)

## 💬 Commit Guidelines

### Commit Message Format

We follow the Conventional Commits specification:

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- `feat`: A new feature
- `fix`: A bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, missing semicolons, etc.)
- `refactor`: Code refactoring without changing functionality
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

### Examples

```bash
feat(customer): add customer search functionality

fix(payment): resolve decimal rounding issue in payment calculation

docs(readme): update installation instructions

refactor(repository): simplify query logic in CustomerRepository
```

## 🔍 Code Review Guidelines

### Before Submitting a PR

- [ ] Code builds without errors
- [ ] All existing tests pass
- [ ] New features have appropriate tests
- [ ] Code follows project conventions
- [ ] No debugging code left in (e.g., `Console.WriteLine`)
- [ ] Comments are clear and helpful

### During Code Review

- Review code thoroughly
- Provide constructive feedback
- Ask questions if something is unclear
- Suggest improvements, don't just criticize
- Approve when satisfied

## 🧪 Testing

### Writing Tests

- Write unit tests for business logic in Service layer
- Write integration tests for Repository layer
- Maintain test coverage above 70%

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests for specific project
dotnet test ASM1.Tests/ASM1.Tests.csproj
```

## 🐛 Reporting Bugs

When reporting bugs, please include:

1. **Description**: Clear description of the issue
2. **Steps to Reproduce**: Detailed steps to reproduce the bug
3. **Expected Behavior**: What should happen
4. **Actual Behavior**: What actually happens
5. **Screenshots**: If applicable
6. **Environment**: OS, .NET version, browser (if applicable)

## 💡 Suggesting Features

When suggesting new features:

1. Check if the feature already exists
2. Clearly describe the feature and its benefits
3. Explain use cases
4. Consider implementation complexity

## 📞 Getting Help

- Ask your team members
- Consult the instructor or TA
- Check official documentation:
  - [ASP.NET Core Docs](https://docs.microsoft.com/en-us/aspnet/core/)
  - [Entity Framework Core Docs](https://docs.microsoft.com/en-us/ef/core/)

## 🎓 Learning Resources

- Microsoft Learn: ASP.NET Core tutorials
- Entity Framework Core documentation
- Clean Architecture principles
- SOLID principles

---

**Remember**: This is a learning project. Don't be afraid to make mistakes, ask questions, and learn from feedback!

