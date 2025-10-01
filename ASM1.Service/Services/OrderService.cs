using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await Task.FromResult(_orderRepository.GetAllOrders());
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await Task.FromResult(_orderRepository.GetOrderById(id));
        }

        public async Task<Order?> CreateOrderAsync(Order order)
        {
            if (!await ValidateOrderAsync(order))
                return null;

            if (!await CanCreateOrderAsync(order.DealerId, order.CustomerId, order.VariantId))
                return null;

            _orderRepository.AddOrder(order);
            return order;
        }

        public async Task<Order?> UpdateOrderAsync(Order order)
        {
            if (!await ValidateOrderAsync(order))
                return null;

            _orderRepository.UpdateOrder(order);
            return order;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            try
            {
                _orderRepository.DeleteOrder(id);
                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByDealerAsync(int dealerId)
        {
            return await Task.FromResult(_orderRepository.GetOrdersByDealer(dealerId));
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            return await Task.FromResult(_orderRepository.GetOrdersByCustomer(customerId));
        }

        public async Task<IEnumerable<Order>> GetOrdersByVariantAsync(int variantId)
        {
            return await Task.FromResult(_orderRepository.GetOrdersByVariant(variantId));
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            return await Task.FromResult(_orderRepository.GetOrdersByStatus(status));
        }

        public async Task<int> GetTotalOrdersByDealerAsync(int dealerId)
        {
            return await Task.FromResult(_orderRepository.GetTotalOrdersByDealer(dealerId));
        }

        public async Task<int> GetTotalOrdersByCustomerAsync(int customerId)
        {
            return await Task.FromResult(_orderRepository.GetTotalOrdersByCustomer(customerId));
        }

        public async Task<decimal> GetTotalOrderValueByDealerAsync(int dealerId)
        {
            return await Task.FromResult(_orderRepository.GetTotalOrderValueByDealer(dealerId));
        }

        public async Task<decimal> GetTotalOrderValueByCustomerAsync(int customerId)
        {
            return await Task.FromResult(_orderRepository.GetTotalOrderValueByCustomer(customerId));
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            try
            {
                _orderRepository.UpdateOrderStatus(orderId, status);
                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await Task.FromResult(_orderRepository.GetPendingOrders());
        }

        public async Task<IEnumerable<Order>> GetCompletedOrdersAsync()
        {
            return await Task.FromResult(_orderRepository.GetCompletedOrders());
        }

        public async Task<bool> ValidateOrderAsync(Order order)
        {
            return await Task.FromResult(
                order != null &&
                order.DealerId > 0 &&
                order.CustomerId > 0 &&
                order.VariantId > 0 &&
                !string.IsNullOrEmpty(order.Status)
            );
        }

        public async Task<bool> CanCreateOrderAsync(int dealerId, int customerId, int variantId)
        {
            // Basic validation - can be extended with business rules
            return await Task.FromResult(
                dealerId > 0 && 
                customerId > 0 && 
                variantId > 0
            );
        }
    }
}
