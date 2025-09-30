using ASM1.Repository.Models;
using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IVehicleModelService
    {
        Task<ServiceResponse<IEnumerable<VehicleModel>>> GetAllAsync();
        Task<ServiceResponse<VehicleModel?>> GetByIdAsync(int id);
        Task<ServiceResponse<bool>> AddAsync(VehicleModel model);
        Task<ServiceResponse<bool>> UpdateAsync(VehicleModel model);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
        Task<ServiceResponse<IEnumerable<VehicleModel>>> GetByManufacturerAsync(int manufacturerId);
    }
}