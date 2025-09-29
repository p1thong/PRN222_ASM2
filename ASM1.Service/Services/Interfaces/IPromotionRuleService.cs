using ASM1.Repository.Repositories;
using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IPromotionRuleService
    {
        Task<PromotionCalculationResult> CalculateApplicablePromotionsAsync(PromotionCalculationRequest request);
        Task<List<PromotionRuleViewModel>> GetActivePromotionRulesAsync();
        Task<List<ApplicablePromotionViewModel>> GetPromotionsForVehicleAsync(int variantId, decimal basePrice);
        Task<List<ApplicablePromotionViewModel>> GetPromotionsForCustomerAsync(int customerId, decimal basePrice = 0);
        Task<List<ApplicablePromotionViewModel>> GetSeasonalPromotionsAsync(decimal basePrice = 0);
    }
}