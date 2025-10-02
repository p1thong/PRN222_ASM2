namespace ASM1.WebMVC.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int DealerId { get; set; }
        public int CustomerId { get; set; }
        public int VariantId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateOnly? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Notes { get; set; }
        
        // Additional properties for display
        public string? CustomerName { get; set; }
        public string? DealerName { get; set; }
        public string? VehicleInfo { get; set; }
    }

    public class OrderCreateViewModel
    {
        public int DealerId { get; set; }
        public int CustomerId { get; set; }
        public int VariantId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateOnly? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Notes { get; set; }
    }
}
