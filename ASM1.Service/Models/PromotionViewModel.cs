namespace ASM1.Service.Models
{
    public class PromotionRuleViewModel
    {
        public int RuleId { get; set; }
        public string RuleName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PromotionType { get; set; } = null!; // "PERCENTAGE", "FIXED_AMOUNT", "MANUFACTURER", "SEASONAL"
        public decimal DiscountValue { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool IsActive { get; set; }
        
        // Conditions
        public int? ManufacturerId { get; set; }
        public string? ManufacturerName { get; set; }
        public decimal? MinPurchaseAmount { get; set; }
        public decimal? MaxDiscountAmount { get; set; }
        public int? VehicleModelId { get; set; }
        public string? VehicleModelName { get; set; }
        public string? CustomerType { get; set; } // "NEW", "RETURNING", "VIP"
    }

    public class ApplicablePromotionViewModel
    {
        public int RuleId { get; set; }
        public string RuleName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PromotionType { get; set; } = null!;
        public decimal DiscountAmount { get; set; }
        public decimal OriginalDiscountValue { get; set; }
        public string ApplyReason { get; set; } = null!;
        public bool IsAutoApplied { get; set; }
    }

    public class PromotionCalculationRequest
    {
        public int VariantId { get; set; }
        public int CustomerId { get; set; }
        public decimal BasePrice { get; set; }
        public DateTime QuotationDate { get; set; } = DateTime.Now;
    }

    public class PromotionCalculationResult
    {
        public List<ApplicablePromotionViewModel> ApplicablePromotions { get; set; } = new List<ApplicablePromotionViewModel>();
        public List<ApplicablePromotionViewModel> AutoAppliedPromotions { get; set; } = new List<ApplicablePromotionViewModel>();
        public decimal TotalDiscount { get; set; }
        public string DiscountDescription { get; set; } = string.Empty;
    }
}