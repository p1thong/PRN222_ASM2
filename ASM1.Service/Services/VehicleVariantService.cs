using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class VehicleVariantService : IVehicleVariantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleVariantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VehicleVariant>> GetAllAsync()
        {
            return await _unitOfWork.VehicleVariants.GetVariantsWithDetailsAsync();
        }

        public async Task<VehicleVariant?> GetByIdAsync(int id)
        {
            return await _unitOfWork.VehicleVariants.GetVariantWithDetailsAsync(id);
        }

        public async Task AddAsync(VehicleVariant variant)
        {
            await _unitOfWork.VehicleVariants.AddAsync(variant);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleVariant variant)
        {
            await _unitOfWork.VehicleVariants.UpdateAsync(variant);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.VehicleVariants.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<VehicleVariant>> GetByModelAsync(int modelId)
        {
            var variants = await _unitOfWork.VehicleVariants.GetVariantsWithDetailsAsync();
            return variants.Where(v => v.VehicleModelId == modelId).ToList();
        }

        public async Task<IEnumerable<VehicleVariant>> GetByManufacturerAsync(int manufacturerId)
        {
            var variants = await _unitOfWork.VehicleVariants.GetVariantsWithDetailsAsync();
            return variants.Where(v => v.VehicleModel?.ManufacturerId == manufacturerId).ToList();
        }
    }
}
