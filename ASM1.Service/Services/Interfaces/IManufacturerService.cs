using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IManufacturerService
    {
        Task<IEnumerable<Manufacturer>> GetAllAsync();
        Task<Manufacturer?> GetByIdAsync(int id);
        Task AddAsync(Manufacturer manufacturer);
        Task UpdateAsync(Manufacturer manufacturer);
        Task DeleteAsync(int id);
        Task<Manufacturer?> GetByNameAsync(string name);
        
        // Dashboard statistics
        Task<Dictionary<string, object>> GetManufacturerDashboardDataAsync(int manufacturerId);
        Task<int> GetTotalDealersByManufacturerAsync(int manufacturerId);
        Task<int> GetTotalOrdersByManufacturerAsync(int manufacturerId);
        Task<decimal> GetTotalSalesByManufacturerAsync(int manufacturerId);
        Task<decimal> GetTotalTargetSalesByManufacturerAsync(int manufacturerId);
        
        // Dealer performance
        Task<IEnumerable<object>> GetDealerPerformanceAsync(int manufacturerId);
        Task<IEnumerable<object>> GetTopPerformingDealersAsync(int manufacturerId, int topCount = 5);
        Task<IEnumerable<object>> GetUnderperformingDealersAsync(int manufacturerId);
        
        // Order analytics
        Task<IEnumerable<object>> GetOrderAnalyticsByManufacturerAsync(int manufacturerId);
        Task<IEnumerable<object>> GetMonthlySalesTrendAsync(int manufacturerId, int months = 12);
        
        // Contract management
        Task<IEnumerable<DealerContract>> GetActiveContractsByManufacturerAsync(int manufacturerId);
        Task<IEnumerable<DealerContract>> GetExpiringContractsAsync(int manufacturerId, int daysAhead = 30);
        
        // Vehicle model performance
        Task<IEnumerable<object>> GetVehicleModelPerformanceAsync(int manufacturerId);
        Task<IEnumerable<object>> GetTopSellingModelsAsync(int manufacturerId, int topCount = 5);
    }
}
