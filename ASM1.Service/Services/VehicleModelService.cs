using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleModelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VehicleModel>> GetAllAsync()
        {
            return await _unitOfWork.VehicleModels.GetModelsWithVariantsAsync();
        }

        public async Task<VehicleModel?> GetByIdAsync(int id)
        {
            return await _unitOfWork.VehicleModels.GetModelWithVariantsAsync(id);
        }

        public async Task AddAsync(VehicleModel vehicleModel)
        {
            await _unitOfWork.VehicleModels.AddAsync(vehicleModel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleModel vehicleModel)
        {
            await _unitOfWork.VehicleModels.UpdateAsync(vehicleModel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.VehicleModels.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<VehicleModel>> GetByManufacturerAsync(int manufacturerId)
        {
            return await _unitOfWork.VehicleModels.GetModelsByManufacturerAsync(manufacturerId);
        }
    }
}
