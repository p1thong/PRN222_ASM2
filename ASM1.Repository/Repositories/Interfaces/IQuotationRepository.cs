using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IQuotationRepository : IGenericRepository<Quotation>
    {
        Task<int> GenerateUniqueQuotationIdAsync();
    }
}
