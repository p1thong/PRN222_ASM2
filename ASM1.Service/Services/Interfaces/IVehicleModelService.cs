using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IVehicleModelService
    {
        Task<IEnumerable<VehicleModel>> GetAllAsync();
        Task<VehicleModel?> GetByIdAsync(int id);
        Task AddAsync(VehicleModel vehicleModel);
        Task UpdateAsync(VehicleModel vehicleModel);
        Task DeleteAsync(int id);
        Task<IEnumerable<VehicleModel>> GetByManufacturerAsync(int manufacturerId);
    }
}
