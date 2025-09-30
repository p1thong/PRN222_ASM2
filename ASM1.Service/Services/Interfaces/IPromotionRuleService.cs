using ASM1.Repository.Repositories;
using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IPromotionRuleService
    {
        Task<object> CalculateApplicablePromotionsAsync(object request);
        Task<List<object>> GetActivePromotionRulesAsync();
        Task<List<object>> GetPromotionsForVehicleAsync(int variantId, decimal basePrice);
        Task<List<object>> GetPromotionsForCustomerAsync(int customerId, decimal basePrice = 0);
        Task<List<object>> GetSeasonalPromotionsAsync(decimal basePrice = 0);
    }
}