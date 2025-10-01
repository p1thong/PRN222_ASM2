using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class SalesContractService : ISalesContractService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SalesContractService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SalesContract>> GetAllAsync()
        {
            return await _unitOfWork.SalesContracts.GetAllAsync();
        }

        public async Task<SalesContract?> GetByIdAsync(int id)
        {
            return await _unitOfWork.SalesContracts.GetByIdAsync(id);
        }

        public async Task AddAsync(SalesContract contract)
        {
            contract.SaleContractId = await _unitOfWork.SalesContracts.GenerateUniqueSalesContractIdAsync();
            await _unitOfWork.SalesContracts.AddAsync(contract);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(SalesContract contract)
        {
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
