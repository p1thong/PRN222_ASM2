using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Models;
using ASM1.Service.Services.Interfaces;
using AutoMapper;

namespace ASM1.Service.Services
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleModelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<VehicleModel>>> GetAllAsync()
        {
            try
            {
                var models = await _unitOfWork.VehicleModels.GetAllAsync();
                return ServiceResponse<IEnumerable<VehicleModel>>.SuccessResponse(models);
            }
            catch (Exception ex)
            {
                return ServiceResponse<IEnumerable<VehicleModel>>.ErrorResponse("Error getting vehicle models", ex.Message);
            }
        }

        public async Task<ServiceResponse<VehicleModel?>> GetByIdAsync(int id)
        {
            try
            {
                var model = await _unitOfWork.VehicleModels.GetByIdAsync(id);
                return ServiceResponse<VehicleModel?>.SuccessResponse(model);
            }
            catch (Exception ex)
            {
                return ServiceResponse<VehicleModel?>.ErrorResponse("Error getting vehicle model", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> AddAsync(VehicleModel model)
        {
            try
            {
                await _unitOfWork.VehicleModels.AddAsync(model);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Vehicle model added successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error adding vehicle model", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(VehicleModel model)
        {
            try
            {
                await _unitOfWork.VehicleModels.UpdateAsync(model);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Vehicle model updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error updating vehicle model", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.VehicleModels.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Vehicle model deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error deleting vehicle model", ex.Message);
            }
        }

        public async Task<ServiceResponse<IEnumerable<VehicleModel>>> GetByManufacturerAsync(int manufacturerId)
        {
            try
            {
                var models = await _unitOfWork.VehicleModels.GetAllAsync();
                var manufacturerModels = models.Where(m => m.ManufacturerId == manufacturerId);
                return ServiceResponse<IEnumerable<VehicleModel>>.SuccessResponse(manufacturerModels);
            }
            catch (Exception ex)
            {
                return ServiceResponse<IEnumerable<VehicleModel>>.ErrorResponse("Error getting vehicle models by manufacturer", ex.Message);
            }
        }
    }
}