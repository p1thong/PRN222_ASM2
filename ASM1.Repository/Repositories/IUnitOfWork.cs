using ASM1.Repository.Data;
using ASM1.Repository.Repositories.Interfaces;

namespace ASM1.Repository.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        // Add other repositories here
        Task<int> SaveChangesAsync();
    }
}
