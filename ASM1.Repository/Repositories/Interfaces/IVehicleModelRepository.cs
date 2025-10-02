using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IVehicleModelRepository : IGenericRepository<VehicleModel>
    {
        Task<IEnumerable<VehicleModel>> GetModelsByManufacturerAsync(int manufacturerId);
        Task<IEnumerable<VehicleModel>> GetModelsWithVariantsAsync();
        Task<VehicleModel?> GetModelWithVariantsAsync(int modelId);
    }
}