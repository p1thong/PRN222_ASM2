using ASM1.Service.Services.Interfaces;
using ASM1.Service.Models;
using ASM1.WebMVC.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class QuotationController : BaseController
    {
        private readonly IQuotationService _quotationService;
        private readonly ICustomerService _customerService;
        private readonly IVehicleVariantService _vehicleVariantService;

        public QuotationController(IQuotationService quotationService, ICustomerService customerService, IVehicleVariantService vehicleVariantService)
        {
            _quotationService = quotationService;
            _customerService = customerService;
            _vehicleVariantService = vehicleVariantService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _quotationService.GetAllAsync();
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int customerId)
        {
            if (customerId <= 0)
            {
                TempData["Error"] = "Invalid customer ID.";
                return RedirectToAction("Index", "Customer");
            }

            // Get customer details to display in quotation form
            var customerResponse = await _customerService.GetByIdAsync(customerId);
            if (customerResponse == null || !customerResponse.Success)
            {
                TempData["Error"] = "Customer not found.";
                return RedirectToAction("Index", "Customer");
            }

            // Get vehicle variants for dropdown
            var variantsResponse = await _vehicleVariantService.GetAllAsync();
            ViewBag.VehicleVariants = variantsResponse.Success ? variantsResponse.Data : new List<VehicleVariantViewModel>();

            ViewBag.Customer = customerResponse.Data;
            ViewBag.CustomerId = customerId;
            ViewBag.DealerId = GetDealerIdFromSession();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuotationCreateViewModel model)
        {
            try
            {
                // Debug: Log method entry
                Console.WriteLine("=== QUOTATION CREATE POST CALLED ===");
                Console.WriteLine($"Timestamp: {DateTime.Now}");
                
                // Debug: Log received model data
                Console.WriteLine("=== QUOTATION CREATE DEBUG ===");
                Console.WriteLine($"Model received: CustomerId={model.CustomerId}, VariantId={model.VariantId}, DealerId={model.DealerId}");
                Console.WriteLine($"Pricing: BasePrice={model.BasePrice}, DiscountAmount={model.DiscountAmount}, AdditionalFees={model.AdditionalFees}");
                Console.WriteLine($"Tax: TaxRate={model.TaxRate}, Status={model.Status}");
                Console.WriteLine($"Descriptions: Discount='{model.DiscountDescription}', Fees='{model.FeesDescription}'");
                
                // Debug: Check ModelState
                Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Validation errors:");
                    foreach (var error in ModelState)
                    {
                        Console.WriteLine($"  {error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }

                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState invalid, reloading view data...");
                    // Reload customer data and variants if validation fails
                    var customerResponse = await _customerService.GetByIdAsync(model.CustomerId);
                if (customerResponse != null && customerResponse.Success)
                {
                    ViewBag.Customer = customerResponse.Data;
                }
                
                var variantsResponse = await _vehicleVariantService.GetAllAsync();
                ViewBag.VehicleVariants = variantsResponse.Success ? variantsResponse.Data : new List<VehicleVariantViewModel>();
                
                ViewBag.CustomerId = model.CustomerId;
                ViewBag.DealerId = GetDealerIdFromSession();
                return View(model);
            }

            try
            {
                // Set dealer ID from session
                model.DealerId = GetDealerIdFromSession() ?? 0;
                model.CreatedAt = DateTime.Now;
                model.Status = "Pending";

                Console.WriteLine($"Final model before service call: DealerId={model.DealerId}, Status={model.Status}");
                await _quotationService.AddAsync(model);
                Console.WriteLine("Quotation service call successful");
                TempData["Success"] = "Quotation created successfully!";
                return RedirectToAction("Index", "Customer");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in quotation creation: {ex.Message}");
                TempData["Error"] = $"Error creating quotation: {ex.Message}";
                
                // Reload customer data and variants
                var customerResponse = await _customerService.GetByIdAsync(model.CustomerId);
                if (customerResponse != null && customerResponse.Success)
                {
                    ViewBag.Customer = customerResponse.Data;
                }
                
                var variantsResponse = await _vehicleVariantService.GetAllAsync();
                ViewBag.VehicleVariants = variantsResponse.Success ? variantsResponse.Data : new List<VehicleVariantViewModel>();
                
                ViewBag.CustomerId = model.CustomerId;
                ViewBag.DealerId = GetDealerIdFromSession();
                return View(model);
            }

            Console.WriteLine("ModelState valid, proceeding with service call...");
            
            await _quotationService.AddAsync(model);
            Console.WriteLine("Service call completed successfully");
            
            TempData["Success"] = "Báo giá đã được tạo thành công!";
            return RedirectToAction("Index", "Customer");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in Create POST: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            
            TempData["Error"] = "Có lỗi hệ thống xảy ra. Vui lòng thử lại.";
            
            // Reload data for error case
            var customerData = await _customerService.GetByIdAsync(model.CustomerId);
            if (customerData != null && customerData.Success)
            {
                ViewBag.Customer = customerData.Data;
            }
            
            var variantsData = await _vehicleVariantService.GetAllAsync();
            ViewBag.VehicleVariants = variantsData.Success ? variantsData.Data : new List<VehicleVariantViewModel>();
            
            ViewBag.CustomerId = model.CustomerId;
            ViewBag.DealerId = GetDealerIdFromSession();
            
            return View(model);
        }
    }        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var quotation = await _quotationService.GetDetailsByIdAsync(id);
            if (quotation == null)
            {
                TempData["Error"] = "Quotation not found.";
                return RedirectToAction("Index");
            }

            return View(quotation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CalculatePrice([FromBody] QuotationPricingRequest request)
        {
            try
            {
                var pricingResult = await _quotationService.CalculatePricingAsync(request);
                return Json(new { success = true, data = pricingResult });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CalculateWithPromotions([FromBody] dynamic request)
        {
            try
            {
                int variantId = request.variantId;
                int customerId = request.customerId;
                decimal additionalFees = request.additionalFees ?? 0;
                decimal taxRate = request.taxRate ?? 0.1m;

                var pricingResult = await _quotationService.CalculatePricingWithPromotionsAsync(variantId, customerId, additionalFees, taxRate);
                return Json(new { success = true, data = pricingResult });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _quotationService.DeleteAsync(id);
                TempData["Success"] = "Quotation deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting quotation: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}