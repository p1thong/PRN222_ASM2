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

        public async Task<ServiceResponse<IEnumerable<VehicleVariantViewModel>>> GetAllAsync()
        {
            try
            {
                var variants = await _unitOfWork.VehicleVariants.GetVariantsWithDetailsAsync();
                var variantVMs = _mapper.Map<IEnumerable<VehicleVariantViewModel>>(variants);
                return new ServiceResponse<IEnumerable<VehicleVariantViewModel>>
                {
                    Success = true,
                    Data = variantVMs
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<VehicleVariantViewModel>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<VehicleVariantViewModel?>> GetByIdAsync(int id)
        {
            try
            {
                var variant = await _unitOfWork.VehicleVariants.GetVariantWithDetailsAsync(id);
                if (variant == null)
                {
                    return new ServiceResponse<VehicleVariantViewModel?>
                    {
                        Success = false,
                        Message = "Vehicle variant not found"
                    };
                }

                var variantVM = _mapper.Map<VehicleVariantViewModel>(variant);
                return new ServiceResponse<VehicleVariantViewModel?>
                {
                    Success = true,
                    Data = variantVM
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<VehicleVariantViewModel?>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<VehicleVariantDetailViewModel?>> GetDetailByIdAsync(int id)
        {
            try
            {
                var variant = await _unitOfWork.VehicleVariants.GetVariantWithDetailsAsync(id);
                if (variant == null)
                {
                    return new ServiceResponse<VehicleVariantDetailViewModel?>
                    {
                        Success = false,
                        Message = "Vehicle variant not found"
                    };
                }

                var variantDetailVM = _mapper.Map<VehicleVariantDetailViewModel>(variant);
                return new ServiceResponse<VehicleVariantDetailViewModel?>
                {
                    Success = true,
                    Data = variantDetailVM
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<VehicleVariantDetailViewModel?>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> AddAsync(VehicleVariantCreateViewModel variantVM)
        {
            try
            {
                var variant = _mapper.Map<VehicleVariant>(variantVM);
                await _unitOfWork.VehicleVariants.AddAsync(variant);
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

        public async Task<ServiceResponse<bool>> UpdateAsync(VehicleVariantViewModel variantVM)
        {
            try
            {
                var variant = _mapper.Map<VehicleVariant>(variantVM);
                await _unitOfWork.VehicleVariants.UpdateAsync(variant);
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
                await _unitOfWork.VehicleVariants.DeleteAsync(id);
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

        public async Task<ServiceResponse<IEnumerable<VehicleVariantViewModel>>> GetByModelAsync(int modelId)
        {
            try
            {
                var variants = await _unitOfWork.VehicleVariants.GetVariantsByModelAsync(modelId);
                var variantVMs = _mapper.Map<IEnumerable<VehicleVariantViewModel>>(variants);
                return new ServiceResponse<IEnumerable<VehicleVariantViewModel>>
                {
                    Success = true,
                    Data = variantVMs
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<VehicleVariantViewModel>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<VehicleVariantViewModel>>> GetByManufacturerAsync(int manufacturerId)
        {
            try
            {
                var variants = await _unitOfWork.VehicleVariants.GetVariantsByManufacturerAsync(manufacturerId);
                var variantVMs = _mapper.Map<IEnumerable<VehicleVariantViewModel>>(variants);
                return new ServiceResponse<IEnumerable<VehicleVariantViewModel>>
                {
                    Success = true,
                    Data = variantVMs
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<VehicleVariantViewModel>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}