using ASM1.Repository.Repositories;

namespace ASM1.Service.Services.Interfaces
{
    public interface IPromotionRuleService
    {
        // Temporarily disabled - can be implemented later when needed
        // Task<PromotionCalculationResult> CalculateApplicablePromotionsAsync(PromotionCalculationRequest request);
        // Task<List<PromotionRuleViewModel>> GetActivePromotionRulesAsync();
        // Task<List<ApplicablePromotionViewModel>> GetPromotionsForVehicleAsync(int variantId, decimal basePrice);
        // Task<List<ApplicablePromotionViewModel>> GetPromotionsForCustomerAsync(int customerId, decimal basePrice = 0);
        // Task<List<ApplicablePromotionViewModel>> GetSeasonalPromotionsAsync(decimal basePrice = 0);
        
        Task<decimal> CalculateDiscountAsync(int customerId, int variantId, decimal basePrice);
    }
}