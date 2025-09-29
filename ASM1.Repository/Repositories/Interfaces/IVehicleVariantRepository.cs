using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IVehicleVariantRepository : IGenericRepository<VehicleVariant>
    {
        Task<IEnumerable<VehicleVariant>> GetVariantsByModelAsync(int modelId);
        Task<IEnumerable<VehicleVariant>> GetVariantsWithDetailsAsync();
        Task<VehicleVariant?> GetVariantWithDetailsAsync(int variantId);
        Task<IEnumerable<VehicleVariant>> GetVariantsByManufacturerAsync(int manufacturerId);
    }
}