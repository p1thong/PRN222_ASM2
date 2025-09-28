using ASM1.Repository.Data;
using ASM1.Repository.Repositories.Interfaces;

namespace ASM1.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarSalesDbContext _context;
        private ICustomerRepository? _customerRepository;
        private IQuotationRepository? _quotationRepository;
        private IOrderRepository? _orderRepository;
        private ISalesContractRepository? _salesContractRepository;
        private IPaymentRepository? _paymentRepository;

        public UnitOfWork(CarSalesDbContext context)
        {
            _context = context;
        }

        public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);
        public IQuotationRepository Quotations => _quotationRepository ??= new QuotationRepository(_context);
        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context);
        public ISalesContractRepository SalesContracts => _salesContractRepository ??= new SalesContractRepository(_context);
        public IPaymentRepository Payments => _paymentRepository ??= new PaymentRepository(_context);

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
