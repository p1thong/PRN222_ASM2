using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Repository.Utilities;

namespace ASM1.Repository.Repositories
{
    public class SalesContractRepository : GenericRepository<SalesContract>, ISalesContractRepository
    {
        public SalesContractRepository(CarSalesDbContext context) : base(context)
        {
        }
        
        public async Task<int> GenerateUniqueSalesContractIdAsync()
        {
            return await IdGenerator.GenerateUniqueSalesContractIdAsync(_context);
        }
    }
}
