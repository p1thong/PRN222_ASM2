using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<int> GenerateUniqueOrderIdAsync();
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();
        Task<Order?> GetByIdWithDetailsAsync(int id);
        
        // Business operations
        Task<IEnumerable<Order>> GetOrdersByDealerAsync(int dealerId);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersByVariantAsync(int variantId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);
        
        // Statistics
        Task<int> GetTotalOrdersByDealerAsync(int dealerId);
        Task<int> GetTotalOrdersByCustomerAsync(int customerId);
        Task<decimal> GetTotalOrderValueByDealerAsync(int dealerId);
        Task<decimal> GetTotalOrderValueByCustomerAsync(int customerId);
        
        // Order status management
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
        Task<IEnumerable<Order>> GetCompletedOrdersAsync();
    }
}
