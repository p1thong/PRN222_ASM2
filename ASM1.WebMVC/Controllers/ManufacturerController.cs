using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class ManufacturerController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public ManufacturerController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // GET: Manufacturer
        public async Task<IActionResult> Index()
        {
            var manufacturers = await _vehicleService.GetAllManufacturersAsync();
            return View(manufacturers);
        }

        // GET: Manufacturer/Models/5
        public async Task<IActionResult> Models(int id)
        {
            var manufacturer = (await _vehicleService.GetAllManufacturersAsync())
                .FirstOrDefault(m => m.ManufacturerId == id);
            
            if (manufacturer == null)
            {
                return NotFound();
            }

            var vehicleModels = await _vehicleService.GetVehicleModelsByManufacturerAsync(id);
            
            ViewBag.ManufacturerName = manufacturer.Name;
            ViewBag.ManufacturerId = id;
            
            return View(vehicleModels);
        }
    }
}
