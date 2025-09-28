using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;

namespace ASM1.Repository.Repositories
{
    public class SalesContractRepository : GenericRepository<SalesContract>, ISalesContractRepository
    {
        public SalesContractRepository(CarSalesDbContext context) : base(context)
        {
        }
        // Implement custom methods for SalesContract if needed
    }
}
