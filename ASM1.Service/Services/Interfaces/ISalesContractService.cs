using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface ISalesContractService
    {
        Task<IEnumerable<SalesContract>> GetAllAsync();
        Task<SalesContract?> GetByIdAsync(int id);
        Task AddAsync(SalesContract contract);
        Task UpdateAsync(SalesContract contract);
        Task DeleteAsync(int id);
    }
}
