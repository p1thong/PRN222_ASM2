namespace ASM1.Service.Models
{
    public class QuotationViewModel
    {
        public int QuotationId { get; set; }
        public int CustomerId { get; set; }
        public int VariantId { get; set; }
        public int DealerId { get; set; }
        public decimal Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
    }

    public class QuotationCreateViewModel
    {
        public int CustomerId { get; set; }
        public int VariantId { get; set; }
        public int DealerId { get; set; }
        public decimal Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
    }
}
