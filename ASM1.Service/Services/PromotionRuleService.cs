using ASM1.Repository.Repositories;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class PromotionRuleService : IPromotionRuleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PromotionRuleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<decimal> CalculateDiscountAsync(int customerId, int variantId, decimal basePrice)
        {
            // Simplified promotion logic - can be enhanced later
            // For now, return a basic 5% discount
            return Task.FromResult(basePrice * 0.05m);
        }
    }
}