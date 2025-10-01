using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IPromotionRepository
    {
        // CRUD operations
        IEnumerable<Promotion> GetAllPromotions();
        Promotion? GetPromotionById(int id);
        void AddPromotion(Promotion promotion);
        void UpdatePromotion(Promotion promotion);
        void DeletePromotion(int id);
        
        // Business operations
        IEnumerable<Promotion> GetPromotionsByOrder(int orderId);
        IEnumerable<Promotion> GetActivePromotions();
        IEnumerable<Promotion> GetExpiredPromotions();
        IEnumerable<Promotion> GetPromotionsByCode(string promotionCode);
        
        // Validation
        bool IsPromotionCodeValid(string promotionCode);
        bool IsPromotionActive(int promotionId);
        decimal GetTotalDiscountByOrder(int orderId);
    }
}
