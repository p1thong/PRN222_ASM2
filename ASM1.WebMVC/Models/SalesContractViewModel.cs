namespace ASM1.WebMVC.Models
{
    public class SalesContractViewModel
    {
        public int SaleContractId { get; set; }
        public int OrderId { get; set; }
        public DateOnly? SignedDate { get; set; }
    }

    public class SalesContractCreateViewModel
    {
        public int OrderId { get; set; }
        public DateOnly? SignedDate { get; set; }
    }
}
