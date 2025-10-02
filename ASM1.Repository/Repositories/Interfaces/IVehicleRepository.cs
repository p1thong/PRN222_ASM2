using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        // VehicleModel methods
        Task<IEnumerable<VehicleModel>> GetAllVehicleModelsAsync();
        Task<VehicleModel?> GetVehicleModelByIdAsync(int id);
        Task<VehicleModel> CreateVehicleModelAsync(VehicleModel vehicleModel);
        Task<VehicleModel> UpdateVehicleModelAsync(VehicleModel vehicleModel);
        Task<bool> DeleteVehicleModelAsync(int id);
        Task<IEnumerable<VehicleModel>> GetVehicleModelsByManufacturerAsync(int manufacturerId);

        // VehicleVariant methods
        Task<IEnumerable<VehicleVariant>> GetAllVehicleVariantsAsync();
        Task<VehicleVariant?> GetVehicleVariantByIdAsync(int id);
        Task<VehicleVariant> CreateVehicleVariantAsync(VehicleVariant vehicleVariant);
        Task<VehicleVariant> UpdateVehicleVariantAsync(VehicleVariant vehicleVariant);
        Task<bool> DeleteVehicleVariantAsync(int id);
        Task<IEnumerable<VehicleVariant>> GetVariantsByModelIdAsync(int vehicleModelId);

        // Helper methods
        Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync();
        Task<bool> VehicleModelExistsAsync(int id);
        Task<bool> VehicleVariantExistsAsync(int id);
    }
}
