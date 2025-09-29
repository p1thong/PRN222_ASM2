namespace ASM1.Service.Models
{
    public class VehicleVariantViewModel
    {
        public int VariantId { get; set; }
        public int VehicleModelId { get; set; }
        public string Version { get; set; } = null!;
        public string? Color { get; set; }
        public int? ProductYear { get; set; }
        public decimal? Price { get; set; }
        public string ModelName { get; set; } = null!;
        public string ManufacturerName { get; set; } = null!;
        public string FullName => $"{ManufacturerName} {ModelName} {Version}";
    }

    public class VehicleVariantCreateViewModel
    {
        public int VehicleModelId { get; set; }
        public string Version { get; set; } = null!;
        public string? Color { get; set; }
        public int? ProductYear { get; set; }
        public decimal? Price { get; set; }
    }

    public class VehicleVariantDetailViewModel
    {
        public int VariantId { get; set; }
        public int VehicleModelId { get; set; }
        public string Version { get; set; } = null!;
        public string? Color { get; set; }
        public int? ProductYear { get; set; }
        public decimal? Price { get; set; }
        public VehicleModelViewModel VehicleModel { get; set; } = null!;
    }
}