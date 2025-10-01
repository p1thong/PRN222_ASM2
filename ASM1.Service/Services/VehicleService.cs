using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        #region VehicleModel Services

        public async Task<IEnumerable<VehicleModel>> GetAllVehicleModelsAsync()
        {
            return await _vehicleRepository.GetAllVehicleModelsAsync();
        }

        public async Task<VehicleModel?> GetVehicleModelByIdAsync(int id)
        {
            return await _vehicleRepository.GetVehicleModelByIdAsync(id);
        }

        public async Task<VehicleModel?> CreateVehicleModelAsync(VehicleModel vehicleModel)
        {
            if (!await ValidateVehicleModelAsync(vehicleModel))
                return null;

            try
            {
                return await _vehicleRepository.CreateVehicleModelAsync(vehicleModel);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<VehicleModel?> UpdateVehicleModelAsync(VehicleModel vehicleModel)
        {
            if (!await ValidateVehicleModelAsync(vehicleModel))
                return null;

            var existingModel = await _vehicleRepository.GetVehicleModelByIdAsync(vehicleModel.VehicleModelId);
            if (existingModel == null)
                return null;

            try
            {
                return await _vehicleRepository.UpdateVehicleModelAsync(vehicleModel);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteVehicleModelAsync(int id)
        {
            try
            {
                return await _vehicleRepository.DeleteVehicleModelAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<VehicleModel>> GetVehicleModelsByManufacturerAsync(int manufacturerId)
        {
            return await _vehicleRepository.GetVehicleModelsByManufacturerAsync(manufacturerId);
        }

        #endregion

        #region VehicleVariant Services

        public async Task<IEnumerable<VehicleVariant>> GetAllVehicleVariantsAsync()
        {
            return await _vehicleRepository.GetAllVehicleVariantsAsync();
        }

        public async Task<VehicleVariant?> GetVehicleVariantByIdAsync(int id)
        {
            return await _vehicleRepository.GetVehicleVariantByIdAsync(id);
        }

        public async Task<VehicleVariant?> CreateVehicleVariantAsync(VehicleVariant vehicleVariant)
        {
            if (!await ValidateVehicleVariantAsync(vehicleVariant))
                return null;

            try
            {
                return await _vehicleRepository.CreateVehicleVariantAsync(vehicleVariant);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<VehicleVariant?> UpdateVehicleVariantAsync(VehicleVariant vehicleVariant)
        {
            if (!await ValidateVehicleVariantAsync(vehicleVariant))
                return null;

            var existingVariant = await _vehicleRepository.GetVehicleVariantByIdAsync(vehicleVariant.VariantId);
            if (existingVariant == null)
                return null;

            try
            {
                return await _vehicleRepository.UpdateVehicleVariantAsync(vehicleVariant);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteVehicleVariantAsync(int id)
        {
            try
            {
                return await _vehicleRepository.DeleteVehicleVariantAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<VehicleVariant>> GetVariantsByModelIdAsync(int vehicleModelId)
        {
            return await _vehicleRepository.GetVariantsByModelIdAsync(vehicleModelId);
        }

        #endregion

        #region Helper Services

        public async Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync()
        {
            return await _vehicleRepository.GetAllManufacturersAsync();
        }

        public async Task<bool> ValidateVehicleModelAsync(VehicleModel vehicleModel)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(vehicleModel.Name))
                return false;

            if (vehicleModel.ManufacturerId <= 0)
                return false;

            // Check if manufacturer exists
            var manufacturers = await _vehicleRepository.GetAllManufacturersAsync();
            if (!manufacturers.Any(m => m.ManufacturerId == vehicleModel.ManufacturerId))
                return false;

            return true;
        }

        public async Task<bool> ValidateVehicleVariantAsync(VehicleVariant vehicleVariant)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(vehicleVariant.Version))
                return false;

            if (vehicleVariant.VehicleModelId <= 0)
                return false;

            if (vehicleVariant.Price.HasValue && vehicleVariant.Price < 0)
                return false;

            if (vehicleVariant.ProductYear.HasValue)
            {
                var currentYear = DateTime.Now.Year;
                if (vehicleVariant.ProductYear < 1900 || vehicleVariant.ProductYear > currentYear + 2)
                    return false;
            }

            // Check if vehicle model exists
            var vehicleModelExists = await _vehicleRepository.VehicleModelExistsAsync(vehicleVariant.VehicleModelId);
            if (!vehicleModelExists)
                return false;

            return true;
        }

        #endregion
    }
}
