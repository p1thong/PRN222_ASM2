namespace ASM1.Service.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int DealerId { get; set; }
        public int CustomerId { get; set; }
        public int VariantId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateOnly? OrderDate { get; set; }
    }

    public class OrderCreateViewModel
    {
        public int DealerId { get; set; }
        public int CustomerId { get; set; }
        public int VariantId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateOnly? OrderDate { get; set; }
    }
}
