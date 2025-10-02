using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Repository.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(CarSalesDbContext context) : base(context)
        {
        }
        
        public async Task<int> GenerateUniqueOrderIdAsync()
        {
            return await IdGenerator.GenerateUniqueOrderIdAsync(_context);
        }

        public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        {
            var carSalesContext = (CarSalesDbContext)_context;
            return await carSalesContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                    .ThenInclude(m => m.Manufacturer)
                .Include(o => o.Payments)
                .Include(o => o.Promotions)
                .Include(o => o.SalesContracts)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdWithDetailsAsync(int id)
        {
            var carSalesContext = (CarSalesDbContext)_context;
            return await carSalesContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Include(o => o.Payments)
                .Include(o => o.Promotions)
                .Include(o => o.SalesContracts)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByDealerAsync(int dealerId)
        {
            var carSalesContext = (CarSalesDbContext)_context;
            return await carSalesContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Where(o => o.DealerId == dealerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            var carSalesContext = (CarSalesDbContext)_context;
            return await carSalesContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Where(o => o.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByVariantAsync(int variantId)
        {
            var carSalesContext = (CarSalesDbContext)_context;
            return await carSalesContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Where(o => o.VariantId == variantId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            var carSalesContext = (CarSalesDbContext)_context;
            return await carSalesContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Where(o => o.Status == status)
                .ToListAsync();
        }

        public async Task<int> GetTotalOrdersByDealerAsync(int dealerId)
        {
            var carSalesContext = (CarSalesDbContext)_context;
            return await carSalesContext.Orders.CountAsync(o => o.DealerId == dealerId);
        }

        public async Task<int> GetTotalOrdersByCustomerAsync(int customerId)
        {
            var carSalesContext = (CarSalesDbContext)_context;
            return await carSalesContext.Orders.CountAsync(o => o.CustomerId == customerId);
        }

        public async Task<decimal> GetTotalOrderValueByDealerAsync(int dealerId)
        {
            var carSalesContext = (CarSalesDbContext)_context;
            return await carSalesContext.Orders
                .Where(o => o.DealerId == dealerId && o.Variant.Price.HasValue)
                .SumAsync(o => o.Variant.Price!.Value);
        }

        public async Task<decimal> GetTotalOrderValueByCustomerAsync(int customerId)
        {
            var carSalesContext = (CarSalesDbContext)_context;
            return await carSalesContext.Orders
                .Where(o => o.CustomerId == customerId && o.Variant.Price.HasValue)
                .SumAsync(o => o.Variant.Price!.Value);
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var carSalesContext = (CarSalesDbContext)_context;
            var order = await carSalesContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                await carSalesContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await GetOrdersByStatusAsync("Pending");
        }

        public async Task<IEnumerable<Order>> GetCompletedOrdersAsync()
        {
            return await GetOrdersByStatusAsync("Completed");
        }
    }
}
