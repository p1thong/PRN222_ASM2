using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface ISalesContractService
    {
        Task<IEnumerable<SalesContractViewModel>> GetAllAsync();
        Task<SalesContractViewModel?> GetByIdAsync(int id);
        Task AddAsync(SalesContractCreateViewModel contract);
        Task UpdateAsync(SalesContractViewModel contract);
        Task DeleteAsync(int id);
    }
}
