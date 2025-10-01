using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IVehicleModelService
    {
        Task<ServiceResponse<IEnumerable<VehicleModelViewModel>>> GetAllAsync();
        Task<ServiceResponse<VehicleModelViewModel?>> GetByIdAsync(int id);
        Task<ServiceResponse<VehicleModelDetailViewModel?>> GetDetailByIdAsync(int id);
        Task<ServiceResponse<bool>> AddAsync(VehicleModelCreateViewModel model);
        Task<ServiceResponse<bool>> UpdateAsync(VehicleModelViewModel model);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
        Task<ServiceResponse<IEnumerable<VehicleModelViewModel>>> GetByManufacturerAsync(int manufacturerId);
    }
}