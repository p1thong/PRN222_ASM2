namespace ASM1.WebMVC.Models
{
    public class ManufacturerViewModel
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
        public string? Address { get; set; }
        public int VehicleModelCount { get; set; }
    }

    public class ManufacturerCreateViewModel
    {
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
        public string? Address { get; set; }
    }

    public class ManufacturerDetailViewModel
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
        public string? Address { get; set; }
        public List<VehicleModelViewModel> VehicleModels { get; set; } = new();
    }
}