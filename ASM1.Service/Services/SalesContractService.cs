using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Models;
using ASM1.Service.Services.Interfaces;
using AutoMapper;

namespace ASM1.Service.Services
{
    public class SalesContractService : ISalesContractService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SalesContractService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<SalesContract>>> GetAllAsync()
        {
            try
            {
                var contracts = await _unitOfWork.SalesContracts.GetAllAsync();
                return ServiceResponse<IEnumerable<SalesContract>>.SuccessResponse(contracts);
            }
            catch (Exception ex)
            {
                return ServiceResponse<IEnumerable<SalesContract>>.ErrorResponse("Error getting sales contracts", ex.Message);
            }
        }

        public async Task<ServiceResponse<SalesContract?>> GetByIdAsync(int id)
        {
            try
            {
                var contract = await _unitOfWork.SalesContracts.GetByIdAsync(id);
                return ServiceResponse<SalesContract?>.SuccessResponse(contract);
            }
            catch (Exception ex)
            {
                return ServiceResponse<SalesContract?>.ErrorResponse("Error getting sales contract", ex.Message);
            }
        }

        public async Task<ServiceResponse> AddAsync(SalesContract contract)
        {
            try
            {
                await _unitOfWork.SalesContracts.AddAsync(contract);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse.SuccessResponse("Sales contract added successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.ErrorResponse("Error adding sales contract", ex.Message);
            }
        }

        public async Task<ServiceResponse> UpdateAsync(SalesContract contract)
        {
            try
            {
                await _unitOfWork.SalesContracts.UpdateAsync(contract);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse.SuccessResponse("Sales contract updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.ErrorResponse("Error updating sales contract", ex.Message);
            }
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.SalesContracts.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse.SuccessResponse("Sales contract deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.ErrorResponse("Error deleting sales contract", ex.Message);
            }
        }
    }
}