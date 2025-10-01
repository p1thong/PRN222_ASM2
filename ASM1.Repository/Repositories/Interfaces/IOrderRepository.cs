using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<int> GenerateUniqueOrderIdAsync();
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();
        Task<Order?> GetByIdWithDetailsAsync(int id);

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        // CRUD operations
        IEnumerable<Order> GetAllOrders();
        Order? GetOrderById(int id);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int id);
        
        // Business operations
        IEnumerable<Order> GetOrdersByDealer(int dealerId);
        IEnumerable<Order> GetOrdersByCustomer(int customerId);
        IEnumerable<Order> GetOrdersByVariant(int variantId);
        IEnumerable<Order> GetOrdersByStatus(string status);
        
        // Statistics
        int GetTotalOrdersByDealer(int dealerId);
        int GetTotalOrdersByCustomer(int customerId);
        decimal GetTotalOrderValueByDealer(int dealerId);
        decimal GetTotalOrderValueByCustomer(int customerId);
        
        // Order status management
        void UpdateOrderStatus(int orderId, string status);
        IEnumerable<Order> GetPendingOrders();
        IEnumerable<Order> GetCompletedOrders();
    }
}
