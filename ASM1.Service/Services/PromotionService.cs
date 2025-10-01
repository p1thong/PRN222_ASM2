using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;

        public PromotionService(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            return await Task.FromResult(_promotionRepository.GetAllPromotions());
        }

        public async Task<Promotion?> GetPromotionByIdAsync(int id)
        {
            return await Task.FromResult(_promotionRepository.GetPromotionById(id));
        }

        public async Task<Promotion?> CreatePromotionAsync(Promotion promotion)
        {
            if (!await ValidatePromotionAsync(promotion))
                return null;

            _promotionRepository.AddPromotion(promotion);
            return promotion;
        }

        public async Task<Promotion?> UpdatePromotionAsync(Promotion promotion)
        {
            if (!await ValidatePromotionAsync(promotion))
                return null;

            _promotionRepository.UpdatePromotion(promotion);
            return promotion;
        }

        public async Task<bool> DeletePromotionAsync(int id)
        {
            try
            {
                _promotionRepository.DeletePromotion(id);
                return await Task.FromResult(true);
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsByOrderAsync(int orderId)
        {
            return await Task.FromResult(_promotionRepository.GetPromotionsByOrder(orderId));
        }

        public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync()
        {
            return await Task.FromResult(_promotionRepository.GetActivePromotions());
        }

        public async Task<IEnumerable<Promotion>> GetExpiredPromotionsAsync()
        {
            return await Task.FromResult(_promotionRepository.GetExpiredPromotions());
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsByCodeAsync(string promotionCode)
        {
            return await Task.FromResult(_promotionRepository.GetPromotionsByCode(promotionCode));
        }

        public async Task<bool> IsPromotionCodeValidAsync(string promotionCode)
        {
            return await Task.FromResult(_promotionRepository.IsPromotionCodeValid(promotionCode));
        }

        public async Task<bool> IsPromotionActiveAsync(int promotionId)
        {
            return await Task.FromResult(_promotionRepository.IsPromotionActive(promotionId));
        }

        public async Task<decimal> GetTotalDiscountByOrderAsync(int orderId)
        {
            return await Task.FromResult(_promotionRepository.GetTotalDiscountByOrder(orderId));
        }

        public async Task<bool> ValidatePromotionAsync(Promotion promotion)
        {
            return await Task.FromResult(
                promotion != null &&
                promotion.OrderId > 0 &&
                promotion.DiscountAmount >= 0 &&
                !string.IsNullOrEmpty(promotion.PromotionCode)
            );
        }

        public async Task<bool> CanApplyPromotionAsync(int orderId, string promotionCode)
        {
            var isValidCode = await IsPromotionCodeValidAsync(promotionCode);
            if (!isValidCode)
                return false;

            // Check if promotion is already applied to this order
            var existingPromotions = await GetPromotionsByOrderAsync(orderId);
            return !existingPromotions.Any(p => p.PromotionCode == promotionCode);
        }

        public async Task<decimal> CalculateDiscountAsync(int orderId, string promotionCode)
        {
            var promotions = await GetPromotionsByCodeAsync(promotionCode);
            var activePromotion = promotions.FirstOrDefault(p => 
                p.OrderId == orderId && 
                await IsPromotionActiveAsync(p.PromotionId));

            return activePromotion?.DiscountAmount ?? 0;
        }
    }
}
