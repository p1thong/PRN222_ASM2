using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        // CRUD operations
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<Order?> CreateOrderAsync(Order order);
        Task<Order?> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int id);
        
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
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
        Task<IEnumerable<Order>> GetCompletedOrdersAsync();
        
        // Validation
        Task<bool> ValidateOrderAsync(Order order);
        Task<bool> CanCreateOrderAsync(int dealerId, int customerId, int variantId);
    }
}
