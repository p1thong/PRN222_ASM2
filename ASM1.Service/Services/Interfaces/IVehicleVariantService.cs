using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IVehicleVariantService
    {
        Task<IEnumerable<VehicleVariant>> GetAllAsync();
        Task<VehicleVariant?> GetByIdAsync(int id);
        Task AddAsync(VehicleVariant variant);
        Task UpdateAsync(VehicleVariant variant);
        Task DeleteAsync(int id);
        Task<IEnumerable<VehicleVariant>> GetByModelAsync(int modelId);
        Task<IEnumerable<VehicleVariant>> GetByManufacturerAsync(int manufacturerId);
    }
}