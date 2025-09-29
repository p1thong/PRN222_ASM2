using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IManufacturerService
    {
        Task<ServiceResponse<IEnumerable<ManufacturerViewModel>>> GetAllAsync();
        Task<ServiceResponse<ManufacturerViewModel?>> GetByIdAsync(int id);
        Task<ServiceResponse<ManufacturerDetailViewModel?>> GetDetailByIdAsync(int id);
        Task<ServiceResponse<bool>> AddAsync(ManufacturerCreateViewModel manufacturer);
        Task<ServiceResponse<bool>> UpdateAsync(ManufacturerViewModel manufacturer);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
        Task<ServiceResponse<ManufacturerViewModel?>> GetByNameAsync(string name);
    }
}