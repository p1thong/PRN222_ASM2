namespace ASM1.Service.Models
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateOnly? PaymentDate { get; set; }
    }

    public class PaymentCreateViewModel
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateOnly? PaymentDate { get; set; }
    }
}
