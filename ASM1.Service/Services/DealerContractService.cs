using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class DealerContractService : IDealerContractService
    {
        private readonly IDealerContractRepository _dealerContractRepository;

        public DealerContractService(IDealerContractRepository dealerContractRepository)
        {
            _dealerContractRepository = dealerContractRepository;
        }

        public async Task<IEnumerable<DealerContract>> GetAllDealerContractsAsync()
        {
            return await Task.FromResult(_dealerContractRepository.GetAllDealerContracts());
        }

        public async Task<DealerContract?> GetDealerContractByIdAsync(int id)
        {
            return await Task.FromResult(_dealerContractRepository.GetDealerContractById(id));
        }

        public async Task<DealerContract?> GetDealerContractByDealerAndManufacturerAsync(int dealerId, int manufacturerId)
        {
            return await Task.FromResult(_dealerContractRepository.GetDealerContractByDealerAndManufacturer(dealerId, manufacturerId));
        }

        public async Task<DealerContract?> CreateDealerContractAsync(DealerContract dealerContract)
        {
            if (!await ValidateDealerContractAsync(dealerContract))
                return null;

            if (!await CanCreateContractAsync(dealerContract.DealerId, dealerContract.ManufacturerId))
                return null;

            dealerContract.SignedDate = DateOnly.FromDateTime(DateTime.Now);
            _dealerContractRepository.AddDealerContract(dealerContract);
            return dealerContract;
        }

        public async Task<DealerContract?> UpdateDealerContractAsync(DealerContract dealerContract)
        {
            if (!await ValidateDealerContractAsync(dealerContract))
                return null;

            _dealerContractRepository.UpdateDealerContract(dealerContract);
            return dealerContract;
        }

        public async Task<bool> DeleteDealerContractAsync(int id)
        {
            try
            {
                _dealerContractRepository.DeleteDealerContract(id);
                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<DealerContract>> GetContractsByDealerAsync(int dealerId)
        {
            return await Task.FromResult(_dealerContractRepository.GetContractsByDealer(dealerId));
        }

        public async Task<IEnumerable<DealerContract>> GetContractsByManufacturerAsync(int manufacturerId)
        {
            return await Task.FromResult(_dealerContractRepository.GetContractsByManufacturer(manufacturerId));
        }

        public async Task<bool> IsContractActiveAsync(int dealerId, int manufacturerId)
        {
            return await Task.FromResult(_dealerContractRepository.IsContractActive(dealerId, manufacturerId));
        }

        public async Task<decimal> GetTotalTargetSalesByManufacturerAsync(int manufacturerId)
        {
            return await Task.FromResult(_dealerContractRepository.GetTotalTargetSalesByManufacturer(manufacturerId));
        }

        public async Task<decimal> GetTotalCreditLimitByDealerAsync(int dealerId)
        {
            return await Task.FromResult(_dealerContractRepository.GetTotalCreditLimitByDealer(dealerId));
        }

        public async Task<bool> ValidateDealerContractAsync(DealerContract dealerContract)
        {
            return await Task.FromResult(
                dealerContract != null &&
                dealerContract.DealerId > 0 &&
                dealerContract.ManufacturerId > 0 &&
                dealerContract.TargetSales >= 0 &&
                dealerContract.CreditLimit >= 0
            );
        }

        public async Task<bool> CanCreateContractAsync(int dealerId, int manufacturerId)
        {
            var existingContract = await GetDealerContractByDealerAndManufacturerAsync(dealerId, manufacturerId);
            return existingContract == null;
        }
    }
}
