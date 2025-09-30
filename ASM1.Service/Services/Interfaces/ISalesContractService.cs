using ASM1.Repository.Models;
using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface ISalesContractService
    {
        Task<ServiceResponse<IEnumerable<SalesContract>>> GetAllAsync();
        Task<ServiceResponse<SalesContract?>> GetByIdAsync(int id);
        Task<ServiceResponse> AddAsync(SalesContract contract);
        Task<ServiceResponse> UpdateAsync(SalesContract contract);
        Task<ServiceResponse> DeleteAsync(int id);
    }
}
