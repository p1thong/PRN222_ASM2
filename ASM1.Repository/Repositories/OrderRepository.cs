using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Repository.Utilities;

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
    }
}
