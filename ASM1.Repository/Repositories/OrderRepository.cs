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
                    .ThenInclude(m => m.Manufacturer)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }
    }
}
