using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface ISalesContractRepository : IGenericRepository<SalesContract>
    {
        Task<int> GenerateUniqueSalesContractIdAsync();
    }
}
