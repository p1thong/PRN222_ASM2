using ASM1.Repository.Repositories;
using ASM1.Service.Models;
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

        public async Task<PromotionCalculationResult> CalculateApplicablePromotionsAsync(PromotionCalculationRequest request)
        {
            var result = new PromotionCalculationResult();
            
            // Get all applicable promotions
            var vehiclePromotions = await GetPromotionsForVehicleAsync(request.VariantId, request.BasePrice);
            var customerPromotions = await GetPromotionsForCustomerAsync(request.CustomerId, request.BasePrice);
            var seasonalPromotions = await GetSeasonalPromotionsAsync(request.BasePrice);

            // Combine all promotions
            var allPromotions = new List<ApplicablePromotionViewModel>();
            allPromotions.AddRange(vehiclePromotions);
            allPromotions.AddRange(customerPromotions);
            allPromotions.AddRange(seasonalPromotions);

            // Debug: Always add a test promotion if no others are found
            if (!allPromotions.Any())
            {
                var debugDiscountAmount = request.BasePrice * 0.03m; // 3% debug discount
                allPromotions.Add(new ApplicablePromotionViewModel
                {
                    RuleId = 999,
                    RuleName = "Debug Test Promotion",
                    Description = "Test promotion to verify system works - 3% discount",
                    PromotionType = "PERCENTAGE",
                    DiscountAmount = debugDiscountAmount,
                    OriginalDiscountValue = 3,
                    ApplyReason = "Giảm 3% để test hệ thống",
                    IsAutoApplied = true
                });
            }

            // Remove duplicates and sort by discount amount (highest first)
            result.ApplicablePromotions = allPromotions
                .GroupBy(p => p.RuleId)
                .Select(g => g.First())
                .OrderByDescending(p => p.DiscountAmount)
                .ToList();

            // Auto-apply best promotions (can be configured)
            result.AutoAppliedPromotions = result.ApplicablePromotions
                .Where(p => p.IsAutoApplied)
                .Take(3) // Limit to 3 auto-applied promotions
                .ToList();

            result.TotalDiscount = result.AutoAppliedPromotions.Sum(p => p.DiscountAmount);
            result.DiscountDescription = string.Join("; ", result.AutoAppliedPromotions.Select(p => p.RuleName));

            return result;
        }

        public Task<List<PromotionRuleViewModel>> GetActivePromotionRulesAsync()
        {
            // This would typically come from database, for now we'll use hardcoded rules
            return Task.FromResult(GetHardcodedPromotionRules().Where(r => r.IsActive && r.ValidUntil >= DateTime.Now).ToList());
        }

        public async Task<List<ApplicablePromotionViewModel>> GetPromotionsForVehicleAsync(int variantId, decimal basePrice)
        {
            var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(variantId);
            if (variant == null) return new List<ApplicablePromotionViewModel>();

            var applicablePromotions = new List<ApplicablePromotionViewModel>();
            var promotionRules = GetHardcodedPromotionRules();

            foreach (var rule in promotionRules.Where(r => r.IsActive && r.ValidUntil >= DateTime.Now))
            {
                var promotion = EvaluatePromotionForVehicle(rule, variant, basePrice);
                if (promotion != null)
                {
                    applicablePromotions.Add(promotion);
                }
            }

            return applicablePromotions;
        }

        public async Task<List<ApplicablePromotionViewModel>> GetPromotionsForCustomerAsync(int customerId, decimal basePrice = 0)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
            if (customer == null) return new List<ApplicablePromotionViewModel>();

            var applicablePromotions = new List<ApplicablePromotionViewModel>();
            var promotionRules = GetHardcodedPromotionRules();

            foreach (var rule in promotionRules.Where(r => r.IsActive && r.ValidUntil >= DateTime.Now))
            {
                var promotion = EvaluatePromotionForCustomer(rule, customer, basePrice);
                if (promotion != null)
                {
                    applicablePromotions.Add(promotion);
                }
            }

            return applicablePromotions;
        }

        public Task<List<ApplicablePromotionViewModel>> GetSeasonalPromotionsAsync(decimal basePrice = 0)
        {
            var applicablePromotions = new List<ApplicablePromotionViewModel>();
            var promotionRules = GetHardcodedPromotionRules();
            var now = DateTime.Now;

            foreach (var rule in promotionRules.Where(r => r.IsActive && r.PromotionType == "PERCENTAGE" && r.ValidFrom <= now && r.ValidUntil >= now))
            {
                var discountAmount = basePrice * (rule.DiscountValue / 100);
                if (rule.MaxDiscountAmount.HasValue)
                    discountAmount = Math.Min(discountAmount, rule.MaxDiscountAmount.Value);

                applicablePromotions.Add(new ApplicablePromotionViewModel
                {
                    RuleId = rule.RuleId,
                    RuleName = rule.RuleName,
                    Description = rule.Description,
                    PromotionType = rule.PromotionType,
                    DiscountAmount = discountAmount,
                    OriginalDiscountValue = rule.DiscountValue,
                    ApplyReason = $"Giảm {rule.DiscountValue}% theo mùa",
                    IsAutoApplied = true
                });
            }

            return Task.FromResult(applicablePromotions);
        }

        private ApplicablePromotionViewModel? EvaluatePromotionForVehicle(PromotionRuleViewModel rule, dynamic variant, decimal basePrice)
        {
            // Calculate percentage discount for all promotion types
            var discountAmount = basePrice * (rule.DiscountValue / 100);
            
            // Apply max discount limit if specified
            if (rule.MaxDiscountAmount.HasValue)
                discountAmount = Math.Min(discountAmount, rule.MaxDiscountAmount.Value);

            // Check if this is a general percentage promotion
            if (rule.PromotionType == "PERCENTAGE")
            {
                return new ApplicablePromotionViewModel
                {
                    RuleId = rule.RuleId,
                    RuleName = rule.RuleName,
                    Description = rule.Description,
                    PromotionType = rule.PromotionType,
                    DiscountAmount = discountAmount,
                    OriginalDiscountValue = rule.DiscountValue,
                    ApplyReason = $"Giảm {rule.DiscountValue}% trên tổng giá",
                    IsAutoApplied = true
                };
            }

            // Check manufacturer-specific promotions (but still use percentage)
            if (rule.ManufacturerId.HasValue)
            {
                if (variant.VehicleModel.ManufacturerId == rule.ManufacturerId.Value)
                {
                    return new ApplicablePromotionViewModel
                    {
                        RuleId = rule.RuleId,
                        RuleName = rule.RuleName,
                        Description = rule.Description,
                        PromotionType = rule.PromotionType,
                        DiscountAmount = discountAmount,
                        OriginalDiscountValue = rule.DiscountValue,
                        ApplyReason = $"Giảm {rule.DiscountValue}% cho hãng {rule.ManufacturerName}",
                        IsAutoApplied = true
                    };
                }
            }

            // Check model-specific promotions (but still use percentage)
            if (rule.VehicleModelId.HasValue)
            {
                if (variant.VehicleModelId == rule.VehicleModelId.Value)
                {
                    return new ApplicablePromotionViewModel
                    {
                        RuleId = rule.RuleId,
                        RuleName = rule.RuleName,
                        Description = rule.Description,
                        PromotionType = rule.PromotionType,
                        DiscountAmount = discountAmount,
                        OriginalDiscountValue = rule.DiscountValue,
                        ApplyReason = $"Giảm {rule.DiscountValue}% cho model {rule.VehicleModelName}",
                        IsAutoApplied = true
                    };
                }
            }

            return null;
        }

        private ApplicablePromotionViewModel? EvaluatePromotionForCustomer(PromotionRuleViewModel rule, dynamic customer, decimal basePrice)
        {
            // Check if customer is new (this is a simplified check, in real scenario you would check registration date)
            if (rule.CustomerType == "NEW")
            {
                // Calculate percentage discount for customer promotions too
                var discountAmount = basePrice * (rule.DiscountValue / 100);
                
                // Since we can't easily check customer creation date without modifying database,
                // we'll assume all customers are eligible for new customer promotion
                // In a real scenario, you would check if customer was created within last 30 days
                return new ApplicablePromotionViewModel
                {
                    RuleId = rule.RuleId,
                    RuleName = rule.RuleName,
                    Description = rule.Description,
                    PromotionType = rule.PromotionType,
                    DiscountAmount = discountAmount,
                    OriginalDiscountValue = rule.DiscountValue,
                    ApplyReason = $"Giảm {rule.DiscountValue}% cho khách hàng mới",
                    IsAutoApplied = true
                };
            }

            return null;
        }

        private List<PromotionRuleViewModel> GetHardcodedPromotionRules()
        {
            return new List<PromotionRuleViewModel>
            {
                new PromotionRuleViewModel
                {
                    RuleId = 1,
                    RuleName = "Khuyến mãi tháng 9",
                    Description = "Khuyến mãi đặc biệt trong tháng 9",
                    PromotionType = "PERCENTAGE",
                    DiscountValue = 0,
                    ValidFrom = new DateTime(2025, 9, 1),
                    ValidUntil = new DateTime(2025, 9, 30),
                    IsActive = false, // Disable this promotion
                    MaxDiscountAmount = 0
                },
                new PromotionRuleViewModel
                {
                    RuleId = 2,
                    RuleName = "Khuyến mãi Toyota",
                    Description = "Giảm 30 triệu cho xe Toyota",
                    PromotionType = "MANUFACTURER",
                    DiscountValue = 30000000,
                    ValidFrom = new DateTime(2025, 9, 1),
                    ValidUntil = new DateTime(2025, 12, 31),
                    IsActive = true,
                    ManufacturerId = 1, // Assume Toyota has ID 1
                    ManufacturerName = "Toyota"
                },
                new PromotionRuleViewModel
                {
                    RuleId = 3,
                    RuleName = "Khuyến mãi khách hàng mới",
                    Description = "Giảm 10 triệu cho khách hàng mới",
                    PromotionType = "FIXED_AMOUNT",
                    DiscountValue = 10000000,
                    ValidFrom = new DateTime(2025, 1, 1),
                    ValidUntil = new DateTime(2025, 12, 31),
                    IsActive = true,
                    CustomerType = "NEW"
                },
                new PromotionRuleViewModel
                {
                    RuleId = 4,
                    RuleName = "Khuyến mãi cuối năm",
                    Description = "Giảm 8% cho tất cả xe cuối năm",
                    PromotionType = "SEASONAL",
                    DiscountValue = 40000000, // Fixed 40 triệu
                    ValidFrom = new DateTime(2025, 11, 1),
                    ValidUntil = new DateTime(2025, 12, 31),
                    IsActive = true
                },
                new PromotionRuleViewModel
                {
                    RuleId = 5,
                    RuleName = "Flash Sale",
                    Description = "Giảm 20 triệu - Chương trình có thời hạn",
                    PromotionType = "FIXED_AMOUNT",
                    DiscountValue = 20000000,
                    ValidFrom = new DateTime(2025, 9, 25),
                    ValidUntil = new DateTime(2025, 10, 5),
                    IsActive = true
                }
            };
        }
    }
}