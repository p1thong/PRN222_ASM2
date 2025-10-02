using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _unitOfWork.Orders.GetAllWithDetailsAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);
        }

        public async Task AddAsync(Order order)
        {
            order.OrderId = await _unitOfWork.Orders.GenerateUniqueOrderIdAsync();
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Orders.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByDealerAsync(int dealerId)
        {
            return await _unitOfWork.Orders.GetOrdersByDealerAsync(dealerId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _unitOfWork.Orders.GetOrdersByCustomerAsync(customerId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByVariantAsync(int variantId)
        {
            return await _unitOfWork.Orders.GetOrdersByVariantAsync(variantId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            return await _unitOfWork.Orders.GetOrdersByStatusAsync(status);
        }

        public async Task<int> GetTotalOrdersByDealerAsync(int dealerId)
        {
            return await _unitOfWork.Orders.GetTotalOrdersByDealerAsync(dealerId);
        }

        public async Task<int> GetTotalOrdersByCustomerAsync(int customerId)
        {
            return await _unitOfWork.Orders.GetTotalOrdersByCustomerAsync(customerId);
        }

        public async Task<decimal> GetTotalOrderValueByDealerAsync(int dealerId)
        {
            return await _unitOfWork.Orders.GetTotalOrderValueByDealerAsync(dealerId);
        }

        public async Task<decimal> GetTotalOrderValueByCustomerAsync(int customerId)
        {
            return await _unitOfWork.Orders.GetTotalOrderValueByCustomerAsync(customerId);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            try
            {
                await _unitOfWork.Orders.UpdateOrderStatusAsync(orderId, status);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await _unitOfWork.Orders.GetPendingOrdersAsync();
        }

        public async Task<IEnumerable<Order>> GetCompletedOrdersAsync()
        {
            return await _unitOfWork.Orders.GetCompletedOrdersAsync();
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
