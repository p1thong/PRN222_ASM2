using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Repository.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class QuotationRepository : GenericRepository<Quotation>, IQuotationRepository
    {
        private readonly CarSalesDbContext _dbContext;
        
        public QuotationRepository(CarSalesDbContext context) : base(context)
        {
            _dbContext = context;
        }
        
        public async Task<int> GenerateUniqueQuotationIdAsync()
        {
            return await IdGenerator.GenerateUniqueQuotationIdAsync(_context);
        }
        
        public async Task<Quotation?> GetQuotationWithDetailsAsync(int id)
        {
            return await _dbContext.Quotations
                .Include(q => q.Customer)
                .Include(q => q.Dealer)
                .Include(q => q.Variant)
                    .ThenInclude(v => v.VehicleModel)
                        .ThenInclude(m => m.Manufacturer)
                .FirstOrDefaultAsync(q => q.QuotationId == id);
        }
    }
}
