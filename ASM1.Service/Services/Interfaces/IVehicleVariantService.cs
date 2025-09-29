using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IVehicleVariantService
    {
        Task<ServiceResponse<IEnumerable<VehicleVariantViewModel>>> GetAllAsync();
        Task<ServiceResponse<VehicleVariantViewModel?>> GetByIdAsync(int id);
        Task<ServiceResponse<VehicleVariantDetailViewModel?>> GetDetailByIdAsync(int id);
        Task<ServiceResponse<bool>> AddAsync(VehicleVariantCreateViewModel variant);
        Task<ServiceResponse<bool>> UpdateAsync(VehicleVariantViewModel variant);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
        Task<ServiceResponse<IEnumerable<VehicleVariantViewModel>>> GetByModelAsync(int modelId);
        Task<ServiceResponse<IEnumerable<VehicleVariantViewModel>>> GetByManufacturerAsync(int manufacturerId);
    }
}