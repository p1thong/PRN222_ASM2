using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Models;
using ASM1.Service.Services.Interfaces;
using AutoMapper;

namespace ASM1.Service.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ManufacturerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<ManufacturerViewModel>>> GetAllAsync()
        {
            try
            {
                var manufacturers = await _unitOfWork.Manufacturers.GetManufacturersWithModelsAsync();
                var manufacturerVMs = _mapper.Map<IEnumerable<ManufacturerViewModel>>(manufacturers);
                return new ServiceResponse<IEnumerable<ManufacturerViewModel>>
                {
                    Success = true,
                    Data = manufacturerVMs
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<IEnumerable<ManufacturerViewModel>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<ManufacturerViewModel?>> GetByIdAsync(int id)
        {
            try
            {
                var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(id);
                if (manufacturer == null)
                {
                    return new ServiceResponse<ManufacturerViewModel?>
                    {
                        Success = false,
                        Message = "Manufacturer not found"
                    };
                }

                var manufacturerVM = _mapper.Map<ManufacturerViewModel>(manufacturer);
                return new ServiceResponse<ManufacturerViewModel?>
                {
                    Success = true,
                    Data = manufacturerVM
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ManufacturerViewModel?>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<ManufacturerDetailViewModel?>> GetDetailByIdAsync(int id)
        {
            try
            {
                var manufacturers = await _unitOfWork.Manufacturers.GetManufacturersWithModelsAsync();
                var manufacturer = manufacturers.FirstOrDefault(m => m.ManufacturerId == id);
                
                if (manufacturer == null)
                {
                    return new ServiceResponse<ManufacturerDetailViewModel?>
                    {
                        Success = false,
                        Message = "Manufacturer not found"
                    };
                }

                var manufacturerDetailVM = _mapper.Map<ManufacturerDetailViewModel>(manufacturer);
                return new ServiceResponse<ManufacturerDetailViewModel?>
                {
                    Success = true,
                    Data = manufacturerDetailVM
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ManufacturerDetailViewModel?>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ServiceResponse<bool>> AddAsync(ManufacturerCreateViewModel manufacturerVM)
        {
            try
            {
                var manufacturer = _mapper.Map<Manufacturer>(manufacturerVM);
                await _unitOfWork.Manufacturers.AddAsync(manufacturer);
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

        public async Task<ServiceResponse<bool>> UpdateAsync(ManufacturerViewModel manufacturerVM)
        {
            try
            {
                var manufacturer = _mapper.Map<Manufacturer>(manufacturerVM);
                await _unitOfWork.Manufacturers.UpdateAsync(manufacturer);
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
                await _unitOfWork.Manufacturers.DeleteAsync(id);
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

        public async Task<ServiceResponse<ManufacturerViewModel?>> GetByNameAsync(string name)
        {
            try
            {
                var manufacturer = await _unitOfWork.Manufacturers.GetByNameAsync(name);
                if (manufacturer == null)
                {
                    return new ServiceResponse<ManufacturerViewModel?>
                    {
                        Success = false,
                        Message = "Manufacturer not found"
                    };
                }

                var manufacturerVM = _mapper.Map<ManufacturerViewModel>(manufacturer);
                return new ServiceResponse<ManufacturerViewModel?>
                {
                    Success = true,
                    Data = manufacturerVM
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ManufacturerViewModel?>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}