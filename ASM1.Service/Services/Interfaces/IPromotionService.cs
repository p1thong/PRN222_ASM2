using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IPromotionService
    {
        // CRUD operations
        Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
        Task<Promotion?> GetPromotionByIdAsync(int id);
        Task<Promotion?> CreatePromotionAsync(Promotion promotion);
        Task<Promotion?> UpdatePromotionAsync(Promotion promotion);
        Task<bool> DeletePromotionAsync(int id);
        
        // Business operations
        Task<IEnumerable<Promotion>> GetPromotionsByOrderAsync(int orderId);
        Task<IEnumerable<Promotion>> GetActivePromotionsAsync();
        Task<IEnumerable<Promotion>> GetExpiredPromotionsAsync();
        Task<IEnumerable<Promotion>> GetPromotionsByCodeAsync(string promotionCode);
        
        // Validation
        Task<bool> IsPromotionCodeValidAsync(string promotionCode);
        Task<bool> IsPromotionActiveAsync(int promotionId);
        Task<decimal> GetTotalDiscountByOrderAsync(int orderId);
        
        // Business logic
        Task<bool> ValidatePromotionAsync(Promotion promotion);
        Task<bool> CanApplyPromotionAsync(int orderId, string promotionCode);
        Task<decimal> CalculateDiscountAsync(int orderId, string promotionCode);
    }
}
