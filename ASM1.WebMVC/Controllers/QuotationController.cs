using ASM1.Service.Services.Interfaces;
using AutoMapper;
using ASM1.WebMVC.Models;
using Microsoft.AspNetCore.Mvc;
using ASM1.Repository.Models;

namespace ASM1.WebMVC.Controllers
{
    public class QuotationController : BaseController
    {
        private readonly IQuotationService _quotationService;
        private readonly ICustomerService _customerService;
        private readonly IVehicleVariantService _vehicleVariantService;
        private readonly IMapper _mapper;

        public QuotationController(IQuotationService quotationService, ICustomerService customerService, IVehicleVariantService vehicleVariantService, IMapper mapper)
        {
            _quotationService = quotationService;
            _customerService = customerService;
            _vehicleVariantService = vehicleVariantService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _quotationService.GetAllAsync();
            
            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message;
                return View(new List<QuotationViewModel>());
            }
            
            var quotations = _mapper.Map<IEnumerable<QuotationViewModel>>(response.Data ?? new List<Quotation>());
            return View(quotations.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            // Kiểm tra quyền truy cập
            if (!IsDealerUser())
            {
                TempData["Error"] = "Bạn không có quyền truy cập chức năng này.";
                return RedirectToAction("Index", "Home");
            }

            // Load customers and vehicle variants for dropdowns
            var customersResponse = await _customerService.GetAllAsync();
            var variantsResponse = await _vehicleVariantService.GetAllAsync();

            // Debug: Log customers data
            Console.WriteLine($"DEBUG: CustomersResponse Success: {customersResponse.Success}");
            Console.WriteLine($"DEBUG: Customers Count: {customersResponse.Data?.Count() ?? 0}");
            if (customersResponse.Data != null)
            {
                foreach (var customer in customersResponse.Data.Take(3)) // Show first 3 customers
                {
                    Console.WriteLine($"DEBUG: Customer - ID: {customer.CustomerId}, Name: {customer.FullName}, Email: {customer.Email}");
                }
            }

            ViewBag.Customers = customersResponse.Success ? 
                _mapper.Map<List<CustomerViewModel>>(customersResponse.Data ?? new List<Customer>()) : 
                new List<CustomerViewModel>();
                
            ViewBag.VehicleVariants = variantsResponse.Success ? 
                _mapper.Map<List<VehicleVariantViewModel>>(variantsResponse.Data ?? new List<VehicleVariant>()) : 
                new List<VehicleVariantViewModel>();

            // Debug: Log ViewBag data
            var customers = ViewBag.Customers as List<CustomerViewModel>;
            Console.WriteLine($"DEBUG: ViewBag.Customers Count: {customers?.Count ?? 0}");
            if (customers != null && customers.Any())
            {
                foreach (var customer in customers.Take(3))
                {
                    Console.WriteLine($"DEBUG: ViewBag Customer - ID: {customer.CustomerId}, Name: {customer.FullName}, Email: {customer.Email}");
                }
            }

            return View(new QuotationCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuotationCreateViewModel model)
        {
            // Debug: Log ALL received form data
            Console.WriteLine("=== DEBUG: RECEIVED FORM DATA ===");
            Console.WriteLine($"CustomerId: {model.CustomerId}");
            Console.WriteLine($"VariantId: {model.VariantId}");
            Console.WriteLine($"DealerId: {model.DealerId}");
            Console.WriteLine($"BasePrice: {model.BasePrice}");
            Console.WriteLine($"DiscountAmount: {model.DiscountAmount}");
            Console.WriteLine($"AdditionalFees: {model.AdditionalFees}");
            Console.WriteLine($"TaxRate: {model.TaxRate}");
            Console.WriteLine($"Status: {model.Status}");
            
            // Debug: Check if the values are being passed in Request.Form
            Console.WriteLine("=== DEBUG: REQUEST.FORM VALUES ===");
            foreach (var item in Request.Form)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
            
            if (!ModelState.IsValid)
            {
                // Debug: Log validation errors
                Console.WriteLine("=== DEBUG: MODEL VALIDATION FAILED ===");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                
                // Reload dropdowns
                var customersResponse = await _customerService.GetAllAsync();
                var variantsResponse = await _vehicleVariantService.GetAllAsync();
                
                ViewBag.Customers = customersResponse.Success ? 
                    _mapper.Map<List<CustomerViewModel>>(customersResponse.Data ?? new List<Customer>()) : 
                    new List<CustomerViewModel>();
                    
                ViewBag.VehicleVariants = variantsResponse.Success ? 
                    _mapper.Map<List<VehicleVariantViewModel>>(variantsResponse.Data ?? new List<VehicleVariant>()) : 
                    new List<VehicleVariantViewModel>();

                return View(model);
            }
            
            Console.WriteLine("=== DEBUG: MODEL VALIDATION PASSED ===");

            var dealerId = GetDealerIdFromSession();
            Console.WriteLine($"=== DEBUG: DEALER ID FROM SESSION: {dealerId} ===");
            if (!dealerId.HasValue)
            {
                Console.WriteLine("=== DEBUG: NO DEALER ID - ADDING ERROR ===");
                ModelState.AddModelError(string.Empty, "Không tìm thấy Dealer đang đăng nhập.");
                
                // Reload dropdowns
                var customersResponse = await _customerService.GetAllAsync();
                var variantsResponse = await _vehicleVariantService.GetAllAsync();
                
                ViewBag.Customers = customersResponse.Success ? 
                    _mapper.Map<List<CustomerViewModel>>(customersResponse.Data ?? new List<Customer>()) : 
                    new List<CustomerViewModel>();
                    
                ViewBag.VehicleVariants = variantsResponse.Success ? 
                    _mapper.Map<List<VehicleVariantViewModel>>(variantsResponse.Data ?? new List<VehicleVariant>()) : 
                    new List<VehicleVariantViewModel>();
                    
                return View(model);
            }
            
            Console.WriteLine("=== DEBUG: DEALER ID FOUND - CHECKING VARIANT ===");

            // Get variant to ensure we have the correct base price
            var variantResponse = await _vehicleVariantService.GetByIdAsync(model.VariantId);
            Console.WriteLine($"=== DEBUG: VARIANT RESPONSE - Success: {variantResponse.Success}, Data: {variantResponse.Data?.VariantId} ===");
            if (!variantResponse.Success || variantResponse.Data == null)
            {
                Console.WriteLine("=== DEBUG: VARIANT NOT FOUND - ADDING ERROR ===");
                ModelState.AddModelError(string.Empty, "Không tìm thấy thông tin xe được chọn.");
                
                // Reload dropdowns
                var customersResponse = await _customerService.GetAllAsync();
                var variantsResponse = await _vehicleVariantService.GetAllAsync();
                
                ViewBag.Customers = customersResponse.Success ? 
                    _mapper.Map<List<CustomerViewModel>>(customersResponse.Data ?? new List<Customer>()) : 
                    new List<CustomerViewModel>();
                    
                ViewBag.VehicleVariants = variantsResponse.Success ? 
                    _mapper.Map<List<VehicleVariantViewModel>>(variantsResponse.Data ?? new List<VehicleVariant>()) : 
                    new List<VehicleVariantViewModel>();
                    
                return View(model);
            }

            // Ensure BasePrice is set correctly
            if (model.BasePrice <= 0)
            {
                model.BasePrice = variantResponse.Data.Price ?? 0;
            }

            var quotation = _mapper.Map<Quotation>(model);
            quotation.DealerId = dealerId.Value;
            quotation.Status = "Pending";
            quotation.CreatedAt = DateTime.Now;

            // Debug: Log what we're about to save
            Console.WriteLine("=== DEBUG: ABOUT TO SAVE QUOTATION ===");
            Console.WriteLine($"CustomerId: {quotation.CustomerId}");
            Console.WriteLine($"VariantId: {quotation.VariantId}");
            Console.WriteLine($"DealerId: {quotation.DealerId}");
            Console.WriteLine($"Price: {quotation.Price}");
            Console.WriteLine($"Status: {quotation.Status}");
            Console.WriteLine($"CreatedAt: {quotation.CreatedAt}");

            var response = await _quotationService.AddAsync(quotation);
            
            // Debug: Log service response
            Console.WriteLine("=== DEBUG: SERVICE RESPONSE ===");
            Console.WriteLine($"Success: {response.Success}");
            Console.WriteLine($"Message: {response.Message}");
            if (response.Errors != null && response.Errors.Any())
            {
                Console.WriteLine($"Errors: {string.Join(", ", response.Errors)}");
            }
            
            if (response.Success)
            {
                TempData["SuccessMessage"] = "Tạo quotation thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                // Add the main error message
                ModelState.AddModelError(string.Empty, response.Message);
                
                // Add any additional error details
                if (response.Errors != null && response.Errors.Any())
                {
                    foreach (var error in response.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                
                // Log error for debugging
                TempData["ErrorMessage"] = $"Lỗi tạo quotation: {response.Message}";
                
                // Reload dropdowns
                var customersResponse = await _customerService.GetAllAsync();
                var variantsResponse = await _vehicleVariantService.GetAllAsync();
                
                ViewBag.Customers = customersResponse.Success ? 
                    _mapper.Map<List<CustomerViewModel>>(customersResponse.Data ?? new List<Customer>()) : 
                    new List<CustomerViewModel>();
                    
                ViewBag.VehicleVariants = variantsResponse.Success ? 
                    _mapper.Map<List<VehicleVariantViewModel>>(variantsResponse.Data ?? new List<VehicleVariant>()) : 
                    new List<VehicleVariantViewModel>();

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _quotationService.GetDetailsByIdAsync(id);
            
            if (!response.Success || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Index");
            }

            var quotationViewModel = _mapper.Map<QuotationDetailViewModel>(response.Data);
            return View(quotationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            if (!IsDealerUser())
            {
                TempData["Error"] = "Bạn không có quyền truy cập chức năng này.";
                return RedirectToAction("Index");
            }

            var result = await _quotationService.ApproveAsync(id);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Phê duyệt quotation thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (!IsDealerUser())
            {
                TempData["Error"] = "Bạn không có quyền truy cập chức năng này.";
                return RedirectToAction("Index");
            }

            var result = await _quotationService.CancelAsync(id);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Hủy quotation thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> CalculatePricing(int variantId, int customerId)
        {
            try
            {
                var result = await _quotationService.CalculatePricingAsync(variantId, customerId);
                
                if (result.Success)
                {
                    return Json(new { success = true, price = result.Data });
                }
                else
                {
                    return Json(new { success = false, message = result.Message });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error calculating pricing: " + ex.Message });
            }
        }
    }
}