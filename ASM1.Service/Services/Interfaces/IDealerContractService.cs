using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IDealerContractService
    {
        // CRUD operations
        Task<IEnumerable<DealerContract>> GetAllDealerContractsAsync();
        Task<DealerContract?> GetDealerContractByIdAsync(int id);
        Task<DealerContract?> GetDealerContractByDealerAndManufacturerAsync(int dealerId, int manufacturerId);
        Task<DealerContract?> CreateDealerContractAsync(DealerContract dealerContract);
        Task<DealerContract?> UpdateDealerContractAsync(DealerContract dealerContract);
        Task<bool> DeleteDealerContractAsync(int id);
        
        // Business operations
        Task<IEnumerable<DealerContract>> GetContractsByDealerAsync(int dealerId);
        Task<IEnumerable<DealerContract>> GetContractsByManufacturerAsync(int manufacturerId);
        Task<bool> IsContractActiveAsync(int dealerId, int manufacturerId);
        Task<decimal> GetTotalTargetSalesByManufacturerAsync(int manufacturerId);
        Task<decimal> GetTotalCreditLimitByDealerAsync(int dealerId);
        
        // Validation
        Task<bool> ValidateDealerContractAsync(DealerContract dealerContract);
        Task<bool> CanCreateContractAsync(int dealerId, int manufacturerId);
    }
}
