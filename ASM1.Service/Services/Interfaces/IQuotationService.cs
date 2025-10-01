using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IQuotationService
    {
        Task<IEnumerable<QuotationViewModel>> GetAllAsync();
        Task<QuotationViewModel?> GetByIdAsync(int id);
        Task<IEnumerable<QuotationViewModel>> GetByCustomerIdAsync(int customerId);
        Task<QuotationDetailViewModel?> GetDetailsByIdAsync(int id);
        Task AddAsync(QuotationCreateViewModel quotation);
        Task UpdateAsync(QuotationViewModel quotation);
        Task DeleteAsync(int id);
        Task<bool> ApproveAsync(int id);
        Task<bool> CancelAsync(int id);
        Task<QuotationDetailViewModel> CalculatePricingAsync(QuotationPricingRequest request);
        Task<QuotationDetailViewModel> CalculatePricingWithPromotionsAsync(int variantId, int customerId, decimal additionalFees = 0, decimal taxRate = 0.1m);
    }
}
