using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Repository.Utilities;

namespace ASM1.Repository.Repositories
{
    public class QuotationRepository : GenericRepository<Quotation>, IQuotationRepository
    {
        public QuotationRepository(CarSalesDbContext context) : base(context)
        {
        }
        
        public async Task<int> GenerateUniqueQuotationIdAsync()
        {
            return await IdGenerator.GenerateUniqueQuotationIdAsync(_context);
        }
    }
}
