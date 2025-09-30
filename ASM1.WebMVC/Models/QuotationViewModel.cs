using System.ComponentModel.DataAnnotations;

namespace ASM1.WebMVC.Models
{
    public class QuotationViewModel
    {
        public int QuotationId { get; set; }
        public int CustomerId { get; set; }
        public int VariantId { get; set; }
        public int DealerId { get; set; }
        public decimal Price { get; set; } // This will store the final calculated price
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        
        // Navigation properties for display
        public string? CustomerName { get; set; }
        public string? VehicleInfo { get; set; }
        public string? DealerName { get; set; }
    }

    public class QuotationCreateViewModel
    {
        public int? QuotationId { get; set; } // For edit mode
        
        [Required(ErrorMessage = "Vui lòng chọn khách hàng")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng chọn khách hàng")]
        public int CustomerId { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn xe")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn xe")]
        public int VariantId { get; set; }
        
        public int DealerId { get; set; }
        
        [Required(ErrorMessage = "Giá xe không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá xe phải lớn hơn 0")]
        public decimal BasePrice { get; set; } // Base price from vehicle variant
        
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền giảm giá không được âm")]
        public decimal DiscountAmount { get; set; } = 0;
        
        [Range(0, double.MaxValue, ErrorMessage = "Phí bổ sung không được âm")]
        public decimal AdditionalFees { get; set; } = 0;
        
        [Range(0, 1, ErrorMessage = "Thuế suất phải từ 0% đến 100%")]
        public decimal TaxRate { get; set; } = 0.1m; // Default 10% tax
        
        public string? DiscountDescription { get; set; }
        public string? FeesDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        
        // Calculated property for final price
        public decimal FinalPrice 
        { 
            get 
            { 
                var taxAmount = (BasePrice - DiscountAmount + AdditionalFees) * TaxRate;
                return BasePrice - DiscountAmount + AdditionalFees + taxAmount;
            } 
        }
        
        // Helper property to check if this is edit mode
        public bool IsEdit => QuotationId.HasValue && QuotationId.Value > 0;
    }

    public class QuotationDetailViewModel
    {
        public int QuotationId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerEmail { get; set; } = null!;
        public string? CustomerPhone { get; set; }
        public string VehicleBrand { get; set; } = null!;
        public string VehicleModel { get; set; } = null!;
        public string VehicleVersion { get; set; } = null!;
        public string? VehicleColor { get; set; }
        public int? VehicleYear { get; set; }
        public decimal VehicleBasePrice { get; set; } // From VehicleVariant.Price
        public decimal DiscountAmount { get; set; }
        public decimal AdditionalFees { get; set; }
        public decimal TaxRate { get; set; } = 0.1m;
        public decimal TaxAmount => (VehicleBasePrice - Math.Abs(DiscountAmount) + AdditionalFees) * TaxRate;
        public decimal FinalPrice => VehicleBasePrice - Math.Abs(DiscountAmount) + AdditionalFees + TaxAmount;
        public string? DiscountDescription { get; set; }
        public string? FeesDescription { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public string DealerName { get; set; } = null!;
        
        // Price breakdown for display
        public List<PriceBreakdownItem> PriceBreakdown => new List<PriceBreakdownItem>
        {
            new PriceBreakdownItem { Description = "Giá xe gốc", Amount = VehicleBasePrice, Type = "base" },
            new PriceBreakdownItem { Description = $"Giảm giá {(!string.IsNullOrEmpty(DiscountDescription) ? $"({DiscountDescription})" : "")}", Amount = Math.Abs(DiscountAmount), Type = "discount" },
            new PriceBreakdownItem { Description = $"Phí bổ sung {(!string.IsNullOrEmpty(FeesDescription) ? $"({FeesDescription})" : "")}", Amount = AdditionalFees, Type = "fee" },
            new PriceBreakdownItem { Description = $"Thuế ({TaxRate:P0})", Amount = TaxAmount, Type = "tax" }
        };
    }

    public class PriceBreakdownItem
    {
        public string Description { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Type { get; set; } = null!; // "base", "discount", "fee", "tax"
    }

    public class QuotationPricingRequest
    {
        public int VariantId { get; set; }
        public decimal DiscountAmount { get; set; } = 0;
        public decimal AdditionalFees { get; set; } = 0;
        public decimal TaxRate { get; set; } = 0.1m;
        public string? DiscountDescription { get; set; }
        public string? FeesDescription { get; set; }
    }
}
