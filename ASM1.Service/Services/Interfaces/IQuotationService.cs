using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IQuotationService
    {
        Task<IEnumerable<QuotationViewModel>> GetAllAsync();
        Task<QuotationViewModel?> GetByIdAsync(int id);
        Task AddAsync(QuotationCreateViewModel quotation);
        Task UpdateAsync(QuotationViewModel quotation);
        Task DeleteAsync(int id);
    }
}
