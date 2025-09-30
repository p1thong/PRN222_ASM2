using ASM1.Repository.Models;
using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IVehicleVariantService
    {
        Task<ServiceResponse<IEnumerable<VehicleVariant>>> GetAllAsync();
        Task<ServiceResponse<VehicleVariant?>> GetByIdAsync(int id);
        Task<ServiceResponse<bool>> AddAsync(VehicleVariant variant);
        Task<ServiceResponse<bool>> UpdateAsync(VehicleVariant variant);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
        Task<ServiceResponse<IEnumerable<VehicleVariant>>> GetByModelAsync(int modelId);
        Task<ServiceResponse<IEnumerable<VehicleVariant>>> GetByManufacturerAsync(int manufacturerId);
    }
}