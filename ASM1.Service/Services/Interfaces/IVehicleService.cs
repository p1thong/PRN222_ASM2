using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IVehicleService
    {
        // VehicleModel services
        Task<IEnumerable<VehicleModel>> GetAllVehicleModelsAsync();
        Task<VehicleModel?> GetVehicleModelByIdAsync(int id);
        Task<VehicleModel?> CreateVehicleModelAsync(VehicleModel vehicleModel);
        Task<VehicleModel?> UpdateVehicleModelAsync(VehicleModel vehicleModel);
        Task<bool> DeleteVehicleModelAsync(int id);
        Task<IEnumerable<VehicleModel>> GetVehicleModelsByManufacturerAsync(int manufacturerId);

        // VehicleVariant services
        Task<IEnumerable<VehicleVariant>> GetAllVehicleVariantsAsync();
        Task<VehicleVariant?> GetVehicleVariantByIdAsync(int id);
        Task<VehicleVariant?> CreateVehicleVariantAsync(VehicleVariant vehicleVariant);
        Task<VehicleVariant?> UpdateVehicleVariantAsync(VehicleVariant vehicleVariant);
        Task<bool> DeleteVehicleVariantAsync(int id);
        Task<IEnumerable<VehicleVariant>> GetVariantsByModelIdAsync(int vehicleModelId);

        // Helper services
        Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync();
        Task<bool> ValidateVehicleModelAsync(VehicleModel vehicleModel);
        Task<bool> ValidateVehicleVariantAsync(VehicleVariant vehicleVariant);
    }
}
