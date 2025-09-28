using ASM1.Repository.Data;
using ASM1.Repository.Repositories.Interfaces;

namespace ASM1.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarSalesDbContext _context;
        private ICustomerRepository? _customerRepository;
        // Add other repositories here

        public UnitOfWork(CarSalesDbContext context)
        {
            _context = context;
        }

        public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);
        // Add other repositories' properties here

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
