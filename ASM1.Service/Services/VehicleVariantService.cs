using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Models;
using ASM1.Service.Services.Interfaces;
using AutoMapper;

namespace ASM1.Service.Services
{
    public class VehicleVariantService : IVehicleVariantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleVariantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<VehicleVariant>>> GetAllAsync()
        {
            try
            {
                var variants = await _unitOfWork.VehicleVariants.GetAllAsync();
                return ServiceResponse<IEnumerable<VehicleVariant>>.SuccessResponse(variants);
            }
            catch (Exception ex)
            {
                return ServiceResponse<IEnumerable<VehicleVariant>>.ErrorResponse("Error getting vehicle variants", ex.Message);
            }
        }

        public async Task<ServiceResponse<VehicleVariant?>> GetByIdAsync(int id)
        {
            try
            {
                var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(id);
                return ServiceResponse<VehicleVariant?>.SuccessResponse(variant);
            }
            catch (Exception ex)
            {
                return ServiceResponse<VehicleVariant?>.ErrorResponse("Error getting vehicle variant", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> AddAsync(VehicleVariant variant)
        {
            try
            {
                await _unitOfWork.VehicleVariants.AddAsync(variant);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Vehicle variant added successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error adding vehicle variant", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(VehicleVariant variant)
        {
            try
            {
                await _unitOfWork.VehicleVariants.UpdateAsync(variant);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Vehicle variant updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error updating vehicle variant", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.VehicleVariants.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Vehicle variant deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error deleting vehicle variant", ex.Message);
            }
        }

        public async Task<ServiceResponse<IEnumerable<VehicleVariant>>> GetByModelAsync(int modelId)
        {
            try
            {
                var variants = await _unitOfWork.VehicleVariants.GetAllAsync();
                var modelVariants = variants.Where(v => v.VehicleModelId == modelId);
                return ServiceResponse<IEnumerable<VehicleVariant>>.SuccessResponse(modelVariants);
            }
            catch (Exception ex)
            {
                return ServiceResponse<IEnumerable<VehicleVariant>>.ErrorResponse("Error getting vehicle variants by model", ex.Message);
            }
        }

        public async Task<ServiceResponse<IEnumerable<VehicleVariant>>> GetByManufacturerAsync(int manufacturerId)
        {
            try
            {
                var variants = await _unitOfWork.VehicleVariants.GetAllAsync();
                var manufacturerVariants = variants.Where(v => v.VehicleModel != null && v.VehicleModel.ManufacturerId == manufacturerId);
                return ServiceResponse<IEnumerable<VehicleVariant>>.SuccessResponse(manufacturerVariants);
            }
            catch (Exception ex)
            {
                return ServiceResponse<IEnumerable<VehicleVariant>>.ErrorResponse("Error getting vehicle variants by manufacturer", ex.Message);
            }
        }
    }
}