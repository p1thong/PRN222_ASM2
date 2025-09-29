using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        #region VehicleModel Actions

        // GET: Vehicle/Models
        public async Task<IActionResult> Models()
        {
            var vehicleModels = await _vehicleService.GetAllVehicleModelsAsync();
            return View(vehicleModels);
        }

        // GET: Vehicle/ModelDetails/5
        public async Task<IActionResult> ModelDetails(int id)
        {
            var vehicleModel = await _vehicleService.GetVehicleModelByIdAsync(id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            return View(vehicleModel);
        }

        // GET: Vehicle/CreateModel
        public async Task<IActionResult> CreateModel()
        {
            ViewBag.Manufacturers = await _vehicleService.GetAllManufacturersAsync();
            return View();
        }

        // POST: Vehicle/CreateModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateModel(VehicleModel vehicleModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _vehicleService.CreateVehicleModelAsync(vehicleModel);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Vehicle model created successfully!";
                    return RedirectToAction(nameof(Models));
                }
                
                ModelState.AddModelError("", "Failed to create vehicle model. Please check your input.");
            }

            ViewBag.Manufacturers = await _vehicleService.GetAllManufacturersAsync();
            return View(vehicleModel);
        }

        // GET: Vehicle/EditModel/5
        public async Task<IActionResult> EditModel(int id)
        {
            var vehicleModel = await _vehicleService.GetVehicleModelByIdAsync(id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            ViewBag.Manufacturers = await _vehicleService.GetAllManufacturersAsync();
            return View(vehicleModel);
        }

        // POST: Vehicle/EditModel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditModel(int id, VehicleModel vehicleModel)
        {
            if (id != vehicleModel.VehicleModelId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var result = await _vehicleService.UpdateVehicleModelAsync(vehicleModel);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Vehicle model updated successfully!";
                    return RedirectToAction(nameof(Models));
                }
                
                ModelState.AddModelError("", "Failed to update vehicle model. Please check your input.");
            }

            ViewBag.Manufacturers = await _vehicleService.GetAllManufacturersAsync();
            return View(vehicleModel);
        }

        // GET: Vehicle/DeleteModel/5
        public async Task<IActionResult> DeleteModel(int id)
        {
            var vehicleModel = await _vehicleService.GetVehicleModelByIdAsync(id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            return View(vehicleModel);
        }

        // POST: Vehicle/DeleteModel/5
        [HttpPost, ActionName("DeleteModel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteModelConfirmed(int id)
        {
            var result = await _vehicleService.DeleteVehicleModelAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Vehicle model deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete vehicle model.";
            }

            return RedirectToAction(nameof(Models));
        }

        #endregion

        #region VehicleVariant Actions

        // GET: Vehicle/Variants
        public async Task<IActionResult> Variants(int? modelId)
        {
            if (modelId.HasValue)
            {
                var vehicleModel = await _vehicleService.GetVehicleModelByIdAsync(modelId.Value);
                if (vehicleModel == null)
                {
                    return NotFound();
                }

                var variants = await _vehicleService.GetVariantsByModelIdAsync(modelId.Value);
                ViewBag.VehicleModel = vehicleModel;
                return View(variants);
            }

            var vehicleVariants = await _vehicleService.GetAllVehicleVariantsAsync();
            return View(vehicleVariants);
        }

        // GET: Vehicle/Variants/ByModel/5
        public async Task<IActionResult> VariantsByModel(int modelId)
        {
            var vehicleModel = await _vehicleService.GetVehicleModelByIdAsync(modelId);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            var variants = await _vehicleService.GetVariantsByModelIdAsync(modelId);
            ViewBag.VehicleModel = vehicleModel;
            return View("Variants", variants);
        }

        // GET: Vehicle/VariantDetails/5
        public async Task<IActionResult> VariantDetails(int id)
        {
            var vehicleVariant = await _vehicleService.GetVehicleVariantByIdAsync(id);
            if (vehicleVariant == null)
            {
                return NotFound();
            }

            return View(vehicleVariant);
        }

        // GET: Vehicle/CreateVariant
        public async Task<IActionResult> CreateVariant(int? modelId)
        {
            var vehicleModels = await _vehicleService.GetAllVehicleModelsAsync();
            ViewBag.VehicleModels = vehicleModels;
            
            var variant = new VehicleVariant();
            if (modelId.HasValue)
            {
                variant.VehicleModelId = modelId.Value;
            }
            
            return View(variant);
        }

        // POST: Vehicle/CreateVariant
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVariant(VehicleVariant vehicleVariant)
        {
            if (ModelState.IsValid)
            {
                var result = await _vehicleService.CreateVehicleVariantAsync(vehicleVariant);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Vehicle variant created successfully!";
                    return RedirectToAction(nameof(Variants));
                }
                
                ModelState.AddModelError("", "Failed to create vehicle variant. Please check your input.");
            }

            ViewBag.VehicleModels = await _vehicleService.GetAllVehicleModelsAsync();
            return View(vehicleVariant);
        }

        // GET: Vehicle/EditVariant/5
        public async Task<IActionResult> EditVariant(int id)
        {
            var vehicleVariant = await _vehicleService.GetVehicleVariantByIdAsync(id);
            if (vehicleVariant == null)
            {
                return NotFound();
            }

            ViewBag.VehicleModels = await _vehicleService.GetAllVehicleModelsAsync();
            return View(vehicleVariant);
        }

        // POST: Vehicle/EditVariant/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVariant(int id, VehicleVariant vehicleVariant)
        {
            if (id != vehicleVariant.VariantId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var result = await _vehicleService.UpdateVehicleVariantAsync(vehicleVariant);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Vehicle variant updated successfully!";
                    return RedirectToAction(nameof(Variants));
                }
                
                ModelState.AddModelError("", "Failed to update vehicle variant. Please check your input.");
            }

            ViewBag.VehicleModels = await _vehicleService.GetAllVehicleModelsAsync();
            return View(vehicleVariant);
        }

        // GET: Vehicle/DeleteVariant/5
        public async Task<IActionResult> DeleteVariant(int id)
        {
            var vehicleVariant = await _vehicleService.GetVehicleVariantByIdAsync(id);
            if (vehicleVariant == null)
            {
                return NotFound();
            }

            return View(vehicleVariant);
        }

        // POST: Vehicle/DeleteVariant/5
        [HttpPost, ActionName("DeleteVariant")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVariantConfirmed(int id)
        {
            var result = await _vehicleService.DeleteVehicleVariantAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Vehicle variant deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete vehicle variant.";
            }

            return RedirectToAction(nameof(Variants));
        }

        #endregion
    }
}
