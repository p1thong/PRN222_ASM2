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

            // Create a default model for new quotation
            var model = new QuotationCreateViewModel
            {
                CustomerId = customerId,
                DealerId = GetDealerIdFromSession() ?? 0,
                TaxRate = 0.1m,
                Status = "Pending"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuotationCreateViewModel model)
        {
            try
            {
                // Debug: Print all model values
                Console.WriteLine("=== CREATE POST REQUEST RECEIVED ===");
                Console.WriteLine($"Request received at: {DateTime.Now}");
                Console.WriteLine($"QuotationId: {model.QuotationId}");
                Console.WriteLine($"CustomerId: {model.CustomerId}");
                Console.WriteLine($"VariantId: {model.VariantId}");
                Console.WriteLine($"DealerId: {model.DealerId}");
                Console.WriteLine($"BasePrice: {model.BasePrice}");
                Console.WriteLine($"DiscountAmount: {model.DiscountAmount}");
                Console.WriteLine($"AdditionalFees: {model.AdditionalFees}");
                Console.WriteLine($"TaxRate: {model.TaxRate}");
                Console.WriteLine($"DiscountDescription: '{model.DiscountDescription}'");
                Console.WriteLine($"FeesDescription: '{model.FeesDescription}'");
                Console.WriteLine($"Status: '{model.Status}'");
                Console.WriteLine($"IsEdit: {model.IsEdit}");
                
                // Log form data from Request.Form for debugging
                Console.WriteLine("=== FORM DATA ===");
                foreach (var key in Request.Form.Keys)
                {
                    Console.WriteLine($"{key}: {Request.Form[key]}");
                }

                // Set dealer ID from session if not provided
                if (model.DealerId <= 0)
                {
                    model.DealerId = GetDealerIdFromSession() ?? 1; // Default to 1 if no session
                }

                // Basic validation - only require variant selection
                if (model.VariantId <= 0)
                {
                    TempData["Error"] = "Vui lòng chọn một variant xe.";
                    await LoadViewDataForCreate(model.CustomerId);
                    return View(model);
                }

                // Set default values
                if (string.IsNullOrEmpty(model.Status))
                {
                    model.Status = "Pending";
                }

                // Get variant price if base price is not set
                if (model.BasePrice <= 0)
                {
                    var variant = await _vehicleVariantService.GetByIdAsync(model.VariantId);
                    if (variant.Success && variant.Data?.Price.HasValue == true)
                    {
                        model.BasePrice = variant.Data.Price.Value;
                    }
                    else
                    {
                        TempData["Error"] = "Không thể lấy giá của variant xe được chọn.";
                        await LoadViewDataForCreate(model.CustomerId);
                        return View(model);
                    }
                }

                // Clear ModelState to bypass complex validation
                ModelState.Clear();

                if (model.IsEdit && model.QuotationId.HasValue)
                {
                    // Update existing quotation
                    var quotationToUpdate = new QuotationViewModel
                    {
                        QuotationId = model.QuotationId.Value,
                        CustomerId = model.CustomerId,
                        VariantId = model.VariantId,
                        DealerId = model.DealerId,
                        Price = model.FinalPrice, // Use auto-calculated final price from model
                        CreatedAt = model.CreatedAt,
                        Status = model.Status
                    };

                    try
                    {
                        // Debug information
                        Console.WriteLine($"=== UPDATING QUOTATION ===");
                        Console.WriteLine($"Updating quotation ID: {quotationToUpdate.QuotationId}");
                        Console.WriteLine($"Customer ID: {quotationToUpdate.CustomerId}");
                        Console.WriteLine($"Variant ID: {quotationToUpdate.VariantId}");
                        Console.WriteLine($"Dealer ID: {quotationToUpdate.DealerId}");
                        Console.WriteLine($"Final Price: {model.FinalPrice}");
                        
                        await _quotationService.UpdateAsync(quotationToUpdate);
                        TempData["Success"] = "Báo giá đã được cập nhật thành công!";
                        return RedirectToAction("Details", new { id = model.QuotationId.Value });
                    }
                    catch (Exception updateEx)
                    {
                        Console.WriteLine($"Update error: {updateEx.Message}");
                        TempData["Error"] = $"Lỗi khi cập nhật báo giá: {updateEx.Message}";
                        await LoadViewDataForCreate(model.CustomerId);
                        return View(model);
                    }
                }
                else
                {
                    // Create new quotation
                    model.CreatedAt = DateTime.Now;
                    
                    Console.WriteLine($"=== CREATING NEW QUOTATION ===");
                    Console.WriteLine($"BasePrice: {model.BasePrice}");
                    Console.WriteLine($"DiscountAmount: {model.DiscountAmount}");
                    Console.WriteLine($"AdditionalFees: {model.AdditionalFees}");
                    Console.WriteLine($"TaxRate: {model.TaxRate}");
                    Console.WriteLine($"Final Price: {model.FinalPrice}");
                    
                    try
                    {
                        await _quotationService.AddAsync(model);
                        TempData["Success"] = "Báo giá đã được tạo thành công!";
                        return RedirectToAction("Index", "Customer");
                    }
                    catch (Exception createEx)
                    {
                        Console.WriteLine($"Create error: {createEx.Message}");
                        TempData["Error"] = $"Lỗi khi tạo báo giá: {createEx.Message}";
                        await LoadViewDataForCreate(model.CustomerId);
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
                TempData["Error"] = $"Có lỗi hệ thống xảy ra: {ex.Message}";
                await LoadViewDataForCreate(model.CustomerId);
                return View(model);
            }
        }

        private async Task LoadViewDataForCreate(int customerId)
        {
            // Load customer data
            var customerResponse = await _customerService.GetByIdAsync(customerId);
            if (customerResponse != null && customerResponse.Success)
            {
                ViewBag.Customer = customerResponse.Data;
            }

            // Load vehicle variants
            var variantsResponse = await _vehicleVariantService.GetAllAsync();
            ViewBag.VehicleVariants = variantsResponse.Success ? variantsResponse.Data : new List<VehicleVariantViewModel>();

            ViewBag.CustomerId = customerId;
            ViewBag.DealerId = GetDealerIdFromSession();
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
                // Debug: log incoming request
                Console.WriteLine($"CalculateWithPromotions request: {request}");
                
                int variantId = request.variantId;
                int customerId = request.customerId;
                decimal additionalFees = request.additionalFees ?? 0;
                decimal taxRate = request.taxRate ?? 0.1m;

                Console.WriteLine($"Parsed values - VariantId: {variantId}, CustomerId: {customerId}, Fees: {additionalFees}, Tax: {taxRate}");

                if (customerId <= 0)
                {
                    return Json(new { success = false, message = "Customer ID is invalid" });
                }

                if (variantId <= 0)
                {
                    return Json(new { success = false, message = "Variant ID is invalid" });
                }

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

            // Convert QuotationViewModel to QuotationCreateViewModel
            // Get the vehicle variant to get base price
            var variantResponse = await _vehicleVariantService.GetByIdAsync(quotation.VariantId);
            var basePrice = quotation.Price; // Default fallback
            
            if (variantResponse != null && variantResponse.Success && variantResponse.Data != null)
            {
                basePrice = variantResponse.Data.Price ?? quotation.Price;
            }

            var editModel = new QuotationCreateViewModel
            {
                QuotationId = id, // Set the ID for edit mode
                CustomerId = quotation.CustomerId,
                VariantId = quotation.VariantId,
                DealerId = quotation.DealerId,
                BasePrice = basePrice, // Use actual base price from variant
                DiscountAmount = 0, // Default values - you might want to calculate these from stored data
                AdditionalFees = 0,
                TaxRate = 0.1m,
                DiscountDescription = "",
                FeesDescription = "",
                CreatedAt = quotation.CreatedAt,
                Status = quotation.Status ?? "Pending"
            };

            return View("Create", editModel);
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