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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Approve(int id)
        {
            try
            {
                var result = await _quotationService.ApproveAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Báo giá đã được duyệt thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không thể duyệt báo giá này." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Cancel(int id)
        {
            try
            {
                var result = await _quotationService.CancelAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Báo giá đã được hủy thành công!" });
                }
                else
                {
                    return Json(new { success = false, message = "Không thể hủy báo giá này." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var quotation = await _quotationService.GetByIdAsync(id);
            if (quotation == null)
            {
                TempData["Error"] = "Quotation not found.";
                return RedirectToAction("Index");
            }

            // Get vehicle variants for dropdown
            var variantsResponse = await _vehicleVariantService.GetAllAsync();
            ViewBag.VehicleVariants = variantsResponse.Success ? variantsResponse.Data : new List<VehicleVariantViewModel>();

            // Get customer info
            var customerResponse = await _customerService.GetByIdAsync(quotation.CustomerId);
            ViewBag.Customer = customerResponse?.Data;

            ViewBag.DealerId = GetDealerIdFromSession();

            return View("Create", quotation); // Reuse Create view for editing
        }

        [HttpGet]
        public async Task<IActionResult> Duplicate(int id)
        {
            var quotation = await _quotationService.GetByIdAsync(id);
            if (quotation == null)
            {
                TempData["Error"] = "Quotation not found.";
                return RedirectToAction("Index");
            }

            // Create a new quotation based on the existing one
            var newQuotation = new QuotationViewModel
            {
                CustomerId = quotation.CustomerId,
                VariantId = quotation.VariantId,
                DealerId = quotation.DealerId,
                Price = quotation.Price,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            // Get vehicle variants for dropdown
            var variantsResponse = await _vehicleVariantService.GetAllAsync();
            ViewBag.VehicleVariants = variantsResponse.Success ? variantsResponse.Data : new List<VehicleVariantViewModel>();

            // Get customer info
            var customerResponse = await _customerService.GetByIdAsync(quotation.CustomerId);
            ViewBag.Customer = customerResponse?.Data;

            ViewBag.DealerId = GetDealerIdFromSession();
            ViewBag.IsDuplicate = true;

            return View("Create", newQuotation);
        }

        [HttpGet]
        public async Task<IActionResult> Print(int id)
        {
            var quotation = await _quotationService.GetDetailsByIdAsync(id);
            if (quotation == null)
            {
                TempData["Error"] = "Quotation not found.";
                return RedirectToAction("Index");
            }

            ViewBag.IsPrintView = true;
            return View("Details", quotation);
        }

        [HttpGet]
        public IActionResult CreateTestData()
        {
            // This is just for testing - in production, remove this method
            return View();
        }

        [HttpPost]
        public IActionResult CreateTestData(string action)
        {
            try
            {
                if (action == "create")
                {
                    // Create a test quotation with sample data
                    var testQuotation = new QuotationDetailViewModel
                    {
                        QuotationId = 7213625, // Use a proper 7-digit ID to match the format
                        CustomerName = "Nguyễn Văn A",
                        CustomerEmail = "nguyenvana@example.com",
                        CustomerPhone = "0123456789",
                        VehicleBrand = "Toyota",
                        VehicleModel = "Camry",
                        VehicleVersion = "2.5Q",
                        VehicleColor = "Trắng Ngọc Trai",
                        VehicleYear = 2024,
                        VehicleBasePrice = 1200000000,
                        DiscountAmount = 50000000,
                        AdditionalFees = 30000000,
                        TaxRate = 0.1m,
                        DiscountDescription = "Khuyến mãi tháng 10",
                        FeesDescription = "Phí đăng ký + Bảo hiểm",
                        CreatedAt = DateTime.Now,
                        Status = "Pending",
                        DealerName = "Lê Thị B"
                    };

                    return View("Details", testQuotation);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error creating test data: {ex.Message}";
                return View();
            }
        }
    }
}