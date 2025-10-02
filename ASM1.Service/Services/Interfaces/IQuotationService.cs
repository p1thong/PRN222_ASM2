using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IQuotationService
    {
        Task<IEnumerable<Quotation>> GetAllAsync();
        Task<Quotation?> GetByIdAsync(int id);
        Task<IEnumerable<Quotation>> GetByCustomerIdAsync(int customerId);
        Task AddAsync(Quotation quotation);
        Task UpdateAsync(Quotation quotation);
        Task DeleteAsync(int id);
        Task<bool> ApproveAsync(int id);
        Task<bool> CancelAsync(int id);
    }
}
