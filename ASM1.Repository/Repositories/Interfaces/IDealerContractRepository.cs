using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IDealerContractRepository
    {
        // CRUD operations
        IEnumerable<DealerContract> GetAllDealerContracts();
        DealerContract? GetDealerContractById(int id);
        DealerContract? GetDealerContractByDealerAndManufacturer(int dealerId, int manufacturerId);
        void AddDealerContract(DealerContract dealerContract);
        void UpdateDealerContract(DealerContract dealerContract);
        void DeleteDealerContract(int id);
        
        // Business operations
        IEnumerable<DealerContract> GetContractsByDealer(int dealerId);
        IEnumerable<DealerContract> GetContractsByManufacturer(int manufacturerId);
        bool IsContractActive(int dealerId, int manufacturerId);
        decimal GetTotalTargetSalesByManufacturer(int manufacturerId);
        decimal GetTotalCreditLimitByDealer(int dealerId);

    }
}
