using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly CarSalesDbContext _context;

        public VehicleRepository(CarSalesDbContext context)
        {
            _context = context;
        }

        #region VehicleModel Methods

        public async Task<IEnumerable<VehicleModel>> GetAllVehicleModelsAsync()
        {
            return await _context.VehicleModels
                .Include(vm => vm.Manufacturer)
                .Include(vm => vm.VehicleVariants)
                .OrderBy(vm => vm.Name)
                .ToListAsync();
        }

        public async Task<VehicleModel?> GetVehicleModelByIdAsync(int id)
        {
            return await _context.VehicleModels
                .Include(vm => vm.Manufacturer)
                .Include(vm => vm.VehicleVariants)
                .FirstOrDefaultAsync(vm => vm.VehicleModelId == id);
        }

        public async Task<VehicleModel> CreateVehicleModelAsync(VehicleModel vehicleModel)
        {
            // Generate new ID
            var maxId = await _context.VehicleModels.AnyAsync() 
                ? await _context.VehicleModels.MaxAsync(vm => vm.VehicleModelId)
                : 0;
            vehicleModel.VehicleModelId = maxId + 1;

            _context.VehicleModels.Add(vehicleModel);
            await _context.SaveChangesAsync();
            
            return await GetVehicleModelByIdAsync(vehicleModel.VehicleModelId) ?? vehicleModel;
        }

        public async Task<VehicleModel> UpdateVehicleModelAsync(VehicleModel vehicleModel)
        {
            _context.VehicleModels.Update(vehicleModel);
            await _context.SaveChangesAsync();
            
            return await GetVehicleModelByIdAsync(vehicleModel.VehicleModelId) ?? vehicleModel;
        }

        public async Task<bool> DeleteVehicleModelAsync(int id)
        {
            var vehicleModel = await _context.VehicleModels.FindAsync(id);
            if (vehicleModel == null)
                return false;

            // Check if there are any variants
            var hasVariants = await _context.VehicleVariants.AnyAsync(vv => vv.VehicleModelId == id);
            if (hasVariants)
            {
                // Delete all variants first
                var variants = await _context.VehicleVariants.Where(vv => vv.VehicleModelId == id).ToListAsync();
                _context.VehicleVariants.RemoveRange(variants);
            }

            _context.VehicleModels.Remove(vehicleModel);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<VehicleModel>> GetVehicleModelsByManufacturerAsync(int manufacturerId)
        {
            return await _context.VehicleModels
                .Include(vm => vm.Manufacturer)
                .Include(vm => vm.VehicleVariants)
                .Where(vm => vm.ManufacturerId == manufacturerId)
                .OrderBy(vm => vm.Name)
                .ToListAsync();
        }

        #endregion

        #region VehicleVariant Methods

        public async Task<IEnumerable<VehicleVariant>> GetAllVehicleVariantsAsync()
        {
            return await _context.VehicleVariants
                .Include(vv => vv.VehicleModel)
                .ThenInclude(vm => vm.Manufacturer)
                .OrderBy(vv => vv.VehicleModel.Name)
                .ThenBy(vv => vv.Version)
                .ToListAsync();
        }

        public async Task<VehicleVariant?> GetVehicleVariantByIdAsync(int id)
        {
            return await _context.VehicleVariants
                .Include(vv => vv.VehicleModel)
                .ThenInclude(vm => vm.Manufacturer)
                .FirstOrDefaultAsync(vv => vv.VariantId == id);
        }

        public async Task<VehicleVariant> CreateVehicleVariantAsync(VehicleVariant vehicleVariant)
        {
            // Generate new ID
            var maxId = await _context.VehicleVariants.AnyAsync()
                ? await _context.VehicleVariants.MaxAsync(vv => vv.VariantId)
                : 0;
            vehicleVariant.VariantId = maxId + 1;

            _context.VehicleVariants.Add(vehicleVariant);
            await _context.SaveChangesAsync();
            
            return await GetVehicleVariantByIdAsync(vehicleVariant.VariantId) ?? vehicleVariant;
        }

        public async Task<VehicleVariant> UpdateVehicleVariantAsync(VehicleVariant vehicleVariant)
        {
            _context.VehicleVariants.Update(vehicleVariant);
            await _context.SaveChangesAsync();
            
            return await GetVehicleVariantByIdAsync(vehicleVariant.VariantId) ?? vehicleVariant;
        }

        public async Task<bool> DeleteVehicleVariantAsync(int id)
        {
            var vehicleVariant = await _context.VehicleVariants.FindAsync(id);
            if (vehicleVariant == null)
                return false;

            _context.VehicleVariants.Remove(vehicleVariant);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<VehicleVariant>> GetVariantsByModelIdAsync(int vehicleModelId)
        {
            return await _context.VehicleVariants
                .Include(vv => vv.VehicleModel)
                .Where(vv => vv.VehicleModelId == vehicleModelId)
                .OrderBy(vv => vv.Version)
                .ToListAsync();
        }

        #endregion

        #region Helper Methods

        public async Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync()
        {
            return await _context.Manufacturers
                .OrderBy(m => m.Name)
                .ToListAsync();
        }

        public async Task<bool> VehicleModelExistsAsync(int id)
        {
            return await _context.VehicleModels.AnyAsync(vm => vm.VehicleModelId == id);
        }

        public async Task<bool> VehicleVariantExistsAsync(int id)
        {
            return await _context.VehicleVariants.AnyAsync(vv => vv.VariantId == id);
        }

        #endregion
    }
}
