using ASM1.Repository.Models;
using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IQuotationService
    {
        Task<ServiceResponse<IEnumerable<Quotation>>> GetAllAsync();
        Task<ServiceResponse<Quotation?>> GetByIdAsync(int id);
        Task<ServiceResponse<Quotation?>> GetDetailsByIdAsync(int id);
        Task<ServiceResponse<IEnumerable<Quotation>>> GetByCustomerIdAsync(int customerId);
        Task<ServiceResponse> AddAsync(Quotation quotation);
        Task<ServiceResponse> UpdateAsync(Quotation quotation);
        Task<ServiceResponse> DeleteAsync(int id);
        Task<ServiceResponse<bool>> ApproveAsync(int id);
        Task<ServiceResponse<bool>> CancelAsync(int id);
        Task<ServiceResponse<decimal>> CalculatePricingAsync(int variantId, int customerId);
        Task<ServiceResponse<decimal>> CalculatePricingWithPromotionsAsync(int variantId, int customerId, List<string> promotionCodes);
    }
}
