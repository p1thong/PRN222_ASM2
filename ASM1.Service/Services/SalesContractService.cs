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

        public async Task<IEnumerable<SalesContractViewModel>> GetAllAsync()
        {
            var contracts = await _unitOfWork.SalesContracts.GetAllAsync();
            return _mapper.Map<IEnumerable<SalesContractViewModel>>(contracts);
        }

        public async Task<SalesContractViewModel?> GetByIdAsync(int id)
        {
            var contract = await _unitOfWork.SalesContracts.GetByIdAsync(id);
            return contract == null ? null : _mapper.Map<SalesContractViewModel>(contract);
        }

        public async Task AddAsync(SalesContractCreateViewModel contractVm)
        {
            var contract = _mapper.Map<SalesContract>(contractVm);
            contract.SaleContractId = await _unitOfWork.SalesContracts.GenerateUniqueIdAsync(c => c.SaleContractId);
            await _unitOfWork.SalesContracts.AddAsync(contract);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(SalesContractViewModel contractVm)
        {
            var contract = _mapper.Map<SalesContract>(contractVm);
            await _unitOfWork.SalesContracts.UpdateAsync(contract);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.SalesContracts.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
