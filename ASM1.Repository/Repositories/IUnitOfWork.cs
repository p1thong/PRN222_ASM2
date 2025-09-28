using ASM1.Repository.Data;
using ASM1.Repository.Repositories.Interfaces;

namespace ASM1.Repository.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IQuotationRepository Quotations { get; }
        IOrderRepository Orders { get; }
        ISalesContractRepository SalesContracts { get; }
        IPaymentRepository Payments { get; }
        Task<int> SaveChangesAsync();
    }
}
