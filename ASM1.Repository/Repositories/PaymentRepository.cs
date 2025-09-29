using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Repository.Utilities;

namespace ASM1.Repository.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(CarSalesDbContext context) : base(context)
        {
        }
        
        public async Task<int> GenerateUniquePaymentIdAsync()
        {
            return await IdGenerator.GenerateUniquePaymentIdAsync(_context);
        }
    }
}
