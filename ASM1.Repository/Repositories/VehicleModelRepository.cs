using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class VehicleModelRepository : GenericRepository<VehicleModel>, IVehicleModelRepository
    {
        public VehicleModelRepository(CarSalesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VehicleModel>> GetModelsByManufacturerAsync(int manufacturerId)
        {
            return await _context.Set<VehicleModel>()
                .Where(vm => vm.ManufacturerId == manufacturerId)
                .Include(vm => vm.Manufacturer)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleModel>> GetModelsWithVariantsAsync()
        {
            return await _context.Set<VehicleModel>()
                .Include(vm => vm.Manufacturer)
                .Include(vm => vm.VehicleVariants)
                .ToListAsync();
        }

        public async Task<VehicleModel?> GetModelWithVariantsAsync(int modelId)
        {
            return await _context.Set<VehicleModel>()
                .Include(vm => vm.Manufacturer)
                .Include(vm => vm.VehicleVariants)
                .FirstOrDefaultAsync(vm => vm.VehicleModelId == modelId);
        }
    }
}