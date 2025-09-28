namespace ASM1.Service.Models
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public int DealerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateOnly? Birthday { get; set; }
    }

    // Model dùng cho Add/Create
    public class CustomerCreateViewModel
    {
        public int DealerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateOnly? Birthday { get; set; }
    }
}
