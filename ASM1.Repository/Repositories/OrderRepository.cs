using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CarSalesDbContext _context;

        public OrderRepository(CarSalesDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Include(o => o.Payments)
                .Include(o => o.Promotions)
                .Include(o => o.SalesContracts)
                .ToList();
        }

        public Order? GetOrderById(int id)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Include(o => o.Payments)
                .Include(o => o.Promotions)
                .Include(o => o.SalesContracts)
                .FirstOrDefault(o => o.OrderId == id);
        }

        public void AddOrder(Order order)
        {
            order.OrderDate = DateOnly.FromDateTime(DateTime.Now);
            order.Status = "Pending";
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Order> GetOrdersByDealer(int dealerId)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Where(o => o.DealerId == dealerId)
                .ToList();
        }

        public IEnumerable<Order> GetOrdersByCustomer(int customerId)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Where(o => o.CustomerId == customerId)
                .ToList();
        }

        public IEnumerable<Order> GetOrdersByVariant(int variantId)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Where(o => o.VariantId == variantId)
                .ToList();
        }

        public IEnumerable<Order> GetOrdersByStatus(string status)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Dealer)
                .Include(o => o.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(vm => vm.Manufacturer)
                .Where(o => o.Status == status)
                .ToList();
        }

        public int GetTotalOrdersByDealer(int dealerId)
        {
            return _context.Orders.Count(o => o.DealerId == dealerId);
        }

        public int GetTotalOrdersByCustomer(int customerId)
        {
            return _context.Orders.Count(o => o.CustomerId == customerId);
        }

        public decimal GetTotalOrderValueByDealer(int dealerId)
        {
            return _context.Orders
                .Where(o => o.DealerId == dealerId && o.Variant.Price.HasValue)
                .Sum(o => o.Variant.Price.Value);
        }

        public decimal GetTotalOrderValueByCustomer(int customerId)
        {
            return _context.Orders
                .Where(o => o.CustomerId == customerId && o.Variant.Price.HasValue)
                .Sum(o => o.Variant.Price.Value);
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
            }
        }

        public IEnumerable<Order> GetPendingOrders()
        {
            return GetOrdersByStatus("Pending");
        }

        public IEnumerable<Order> GetCompletedOrders()
        {
            return GetOrdersByStatus("Completed");
        }
    }
}
