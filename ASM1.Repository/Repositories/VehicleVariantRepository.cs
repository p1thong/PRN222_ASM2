using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class VehicleVariantRepository : GenericRepository<VehicleVariant>, IVehicleVariantRepository
    {
        public VehicleVariantRepository(CarSalesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VehicleVariant>> GetVariantsByModelAsync(int modelId)
        {
            return await _context.Set<VehicleVariant>()
                .Where(vv => vv.VehicleModelId == modelId)
                .Include(vv => vv.VehicleModel)
                .ThenInclude(vm => vm.Manufacturer)
                .ToListAsync();
        }

        public async Task<IEnumerable<VehicleVariant>> GetVariantsWithDetailsAsync()
        {
            return await _context.Set<VehicleVariant>()
                .Include(vv => vv.VehicleModel)
                .ThenInclude(vm => vm.Manufacturer)
                .ToListAsync();
        }

        public async Task<VehicleVariant?> GetVariantWithDetailsAsync(int variantId)
        {
            return await _context.Set<VehicleVariant>()
                .Include(vv => vv.VehicleModel)
                .ThenInclude(vm => vm.Manufacturer)
                .FirstOrDefaultAsync(vv => vv.VariantId == variantId);
        }

        public async Task<IEnumerable<VehicleVariant>> GetVariantsByManufacturerAsync(int manufacturerId)
        {
            return await _context.Set<VehicleVariant>()
                .Include(vv => vv.VehicleModel)
                .ThenInclude(vm => vm.Manufacturer)
                .Where(vv => vv.VehicleModel.ManufacturerId == manufacturerId)
                .ToListAsync();
        }
    }
}