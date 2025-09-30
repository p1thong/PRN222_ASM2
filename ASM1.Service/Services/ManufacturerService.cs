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

        public async Task<ServiceResponse<IEnumerable<Manufacturer>>> GetAllAsync()
        {
            try
            {
                var manufacturers = await _unitOfWork.Manufacturers.GetAllAsync();
                return ServiceResponse<IEnumerable<Manufacturer>>.SuccessResponse(manufacturers);
            }
            catch (Exception ex)
            {
                return ServiceResponse<IEnumerable<Manufacturer>>.ErrorResponse("Error getting manufacturers", ex.Message);
            }
        }

        public async Task<ServiceResponse<Manufacturer?>> GetByIdAsync(int id)
        {
            try
            {
                var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(id);
                if (manufacturer == null)
                {
                    return ServiceResponse<Manufacturer?>.ErrorResponse("Manufacturer not found");
                }
                return ServiceResponse<Manufacturer?>.SuccessResponse(manufacturer);
            }
            catch (Exception ex)
            {
                return ServiceResponse<Manufacturer?>.ErrorResponse("Error getting manufacturer", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> AddAsync(Manufacturer manufacturer)
        {
            try
            {
                await _unitOfWork.Manufacturers.AddAsync(manufacturer);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Manufacturer added successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error adding manufacturer", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> UpdateAsync(Manufacturer manufacturer)
        {
            try
            {
                var existingManufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(manufacturer.ManufacturerId);
                if (existingManufacturer == null)
                {
                    return ServiceResponse<bool>.ErrorResponse("Manufacturer not found");
                }

                await _unitOfWork.Manufacturers.UpdateAsync(manufacturer);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Manufacturer updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error updating manufacturer", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            try
            {
                var manufacturer = await _unitOfWork.Manufacturers.GetByIdAsync(id);
                if (manufacturer == null)
                {
                    return ServiceResponse<bool>.ErrorResponse("Manufacturer not found");
                }

                await _unitOfWork.Manufacturers.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Manufacturer deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error deleting manufacturer", ex.Message);
            }
        }

        public async Task<ServiceResponse<Manufacturer?>> GetByNameAsync(string name)
        {
            try
            {
                var manufacturers = await _unitOfWork.Manufacturers.GetAllAsync();
                var manufacturer = manufacturers.FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                return ServiceResponse<Manufacturer?>.SuccessResponse(manufacturer);
            }
            catch (Exception ex)
            {
                return ServiceResponse<Manufacturer?>.ErrorResponse("Error getting manufacturer by name", ex.Message);
            }
        }
    }
}