namespace ASM1.WebMVC.Models
{
    public class VehicleModelViewModel
    {
        public int VehicleModelId { get; set; }
        public int ManufacturerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Category { get; set; }
        public string ManufacturerName { get; set; } = null!;
        public int VariantCount { get; set; }
    }

    public class VehicleModelCreateViewModel
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Category { get; set; }
    }

    public class VehicleModelDetailViewModel
    {
        public int VehicleModelId { get; set; }
        public int ManufacturerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Category { get; set; }
        public string ManufacturerName { get; set; } = null!;
        public List<VehicleVariantViewModel> Variants { get; set; } = new();
    }
}