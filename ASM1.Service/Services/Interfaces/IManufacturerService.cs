using ASM1.Repository.Models;
using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IManufacturerService
    {
        Task<ServiceResponse<IEnumerable<Manufacturer>>> GetAllAsync();
        Task<ServiceResponse<Manufacturer?>> GetByIdAsync(int id);
        Task<ServiceResponse<bool>> AddAsync(Manufacturer manufacturer);
        Task<ServiceResponse<bool>> UpdateAsync(Manufacturer manufacturer);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
        Task<ServiceResponse<Manufacturer?>> GetByNameAsync(string name);
    }
}