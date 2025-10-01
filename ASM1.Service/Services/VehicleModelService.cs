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

        public async Task<ServiceResponse<IEnumerable<VehicleModelViewModel>>> GetAllAsync()
        {
            try
            {
                var models = await _unitOfWork.VehicleModels.GetModelsWithVariantsAsync();
                var modelVMs = _mapper.Map<IEnumerable<VehicleModelViewModel>>(models);
                return new ServiceResponse<IEnumerable<VehicleModelViewModel>>
                {
                    Success = true,
                    Data = modelVMs
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<VehicleModelViewModel>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<VehicleModelViewModel?>> GetByIdAsync(int id)
        {
            try
            {
                var models = await _unitOfWork.VehicleModels.GetModelsWithVariantsAsync();
                var model = models.FirstOrDefault(m => m.VehicleModelId == id);
                
                if (model == null)
                {
                    return new ServiceResponse<VehicleModelViewModel?>
                    {
                        Success = false,
                        Message = "Vehicle model not found"
                    };
                }

                var modelVM = _mapper.Map<VehicleModelViewModel>(model);
                return new ServiceResponse<VehicleModelViewModel?>
                {
                    Success = true,
                    Data = modelVM
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<VehicleModelViewModel?>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<VehicleModelDetailViewModel?>> GetDetailByIdAsync(int id)
        {
            try
            {
                var model = await _unitOfWork.VehicleModels.GetModelWithVariantsAsync(id);
                if (model == null)
                {
                    return new ServiceResponse<VehicleModelDetailViewModel?>
                    {
                        Success = false,
                        Message = "Vehicle model not found"
                    };
                }

                var modelDetailVM = _mapper.Map<VehicleModelDetailViewModel>(model);
                return new ServiceResponse<VehicleModelDetailViewModel?>
                {
                    Success = true,
                    Data = modelDetailVM
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<VehicleModelDetailViewModel?>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> AddAsync(VehicleModelCreateViewModel modelVM)
        {
            try
            {
                var model = _mapper.Map<VehicleModel>(modelVM);
                await _unitOfWork.VehicleModels.AddAsync(model);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceResponse<bool>
                {
                    Success = true,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(VehicleModelViewModel modelVM)
        {
            try
            {
                var model = _mapper.Map<VehicleModel>(modelVM);
                await _unitOfWork.VehicleModels.UpdateAsync(model);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceResponse<bool>
                {
                    Success = true,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.VehicleModels.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceResponse<bool>
                {
                    Success = true,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<VehicleModelViewModel>>> GetByManufacturerAsync(int manufacturerId)
        {
            try
            {
                var models = await _unitOfWork.VehicleModels.GetModelsByManufacturerAsync(manufacturerId);
                var modelVMs = _mapper.Map<IEnumerable<VehicleModelViewModel>>(models);
                return new ServiceResponse<IEnumerable<VehicleModelViewModel>>
                {
                    Success = true,
                    Data = modelVMs
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<VehicleModelViewModel>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}