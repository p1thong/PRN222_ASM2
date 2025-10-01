using ASM1.Service.Services.Interfaces;
using ASM1.WebMVC.Models;
using ASM1.WebMVC.Extensions;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ASM1.Repository.Models;
using ASM1.Service.Models;

namespace ASM1.WebMVC.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IQuotationService _quotationService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IQuotationService quotationService, IMapper mapper)
        {
            _customerService = customerService;
            _quotationService = quotationService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _customerService.GetAllAsync();
            
            if (!response.Success)
            {
                TempData["ErrorMessage"] = response.Message;
                return View(new List<CustomerViewModel>());
            }
            
            var customers = _mapper.Map<IEnumerable<CustomerViewModel>>(response.Data ?? new List<CustomerViewModel>());
            return View(customers.ToList());
        }

        [HttpGet]
        public IActionResult Debug()
        {
            var userId = GetUserIdFromSession();
            var userRole = GetUserRoleFromSession();
            var dealerId = GetDealerIdFromSession();
            var isLoggedIn = IsUserLoggedIn();
            var isDealerUser = IsDealerUser();

            // Console log session data
            Console.WriteLine("=== SESSION DEBUG ===");
            Console.WriteLine($"UserId: {userId}");
            Console.WriteLine($"UserRole: {userRole}");
            Console.WriteLine($"DealerId: {dealerId}");
            Console.WriteLine($"IsLoggedIn: {isLoggedIn}");
            Console.WriteLine($"IsDealerUser: {isDealerUser}");
            Console.WriteLine("All session keys:");
            foreach (var key in HttpContext.Session.Keys)
            {
                Console.WriteLine($"  {key}: {HttpContext.Session.GetString(key) ?? HttpContext.Session.GetInt32(key)?.ToString() ?? "null"}");
            }

            ViewBag.DebugInfo = $"UserId: {userId}, Role: {userRole}, DealerId: {dealerId}, IsLoggedIn: {isLoggedIn}, IsDealerUser: {isDealerUser}";
            
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            // Debug session data
            var userId = GetUserIdFromSession();
            var userRole = GetUserRoleFromSession();
            var dealerId = GetDealerIdFromSession();
            
            // Kiểm tra quyền truy cập
            if (!IsDealerUser())
            {
                TempData["Error"] = $"Bạn không có quyền truy cập chức năng này. UserId: {userId}, Role: {userRole}, DealerId: {dealerId}";
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CustomerCreateViewModel model)
        {
            // Console log dữ liệu nhận được
            Console.WriteLine("=== ADD CUSTOMER DEBUG ===");
            Console.WriteLine($"Model received: FullName={model.FullName}, Email={model.Email}, Phone={model.Phone}, Birthday={model.Birthday}");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState Errors:");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"  {error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(model);
            }

            // Lấy DealerId từ session
            var dealerId = GetDealerIdFromSession();
            var userId = GetUserIdFromSession();
            var userRole = GetUserRoleFromSession();
            
            Console.WriteLine($"Session Data: DealerId={dealerId}, UserId={userId}, UserRole={userRole}");
            
            if (dealerId.HasValue)
            {
                model.DealerId = dealerId.Value;
                Console.WriteLine($"Model before service call: DealerId={model.DealerId}, FullName={model.FullName}, Email={model.Email}");
                
                var customer = _mapper.Map<Customer>(model);
                var response = await _customerService.AddAsync(model);
                Console.WriteLine($"Service response: Success={response.Success}, Message={response.Message}");
                
                if (response.Errors.Any())
                {
                    Console.WriteLine("Service errors:");
                    foreach (var error in response.Errors)
                    {
                        Console.WriteLine($"  - {error}");
                    }
                }
                
                if (response.Success)
                {
                    TempData["SuccessMessage"] = "Thêm khách hàng thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, response.Message);
                    foreach (var error in response.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    return View("Add", model);
                }
            }
            
            Console.WriteLine("No DealerId found in session");
            ModelState.AddModelError(string.Empty, "Không tìm thấy Dealer đang đăng nhập. Vui lòng đăng nhập lại.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _customerService.GetByIdAsync(id);
            
            if (!response.Success || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Index");
            }

            // Lấy danh sách quotations cho customer này
            var quotations = await _quotationService.GetByCustomerIdAsync(id);
            ViewBag.Quotations = quotations;

            var customerViewModel = _mapper.Map<CustomerViewModel>(response.Data);
            return View(customerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            // Kiểm tra quyền truy cập
            if (!IsDealerUser())
            {
                TempData["Error"] = "Bạn không có quyền truy cập chức năng này.";
                return RedirectToAction("Index", "Home");
            }

            var response = await _customerService.GetByIdAsync(id);
            
            if (!response.Success || response.Data == null)
            {
                TempData["ErrorMessage"] = response.Message;
                return RedirectToAction("Index");
            }

            var customerViewModel = _mapper.Map<CustomerViewModel>(response.Data);
            return View(customerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra quyền truy cập
            if (!IsDealerUser())
            {
                TempData["Error"] = "Bạn không có quyền truy cập chức năng này.";
                return RedirectToAction("Index", "Home");
            }

            var customer = _mapper.Map<Customer>(model);
            var response = await _customerService.UpdateAsync(model);
            
            if (response.Success)
            {
                TempData["SuccessMessage"] = "Cập nhật khách hàng thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, response.Message);
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Kiểm tra quyền truy cập
            if (!IsDealerUser())
            {
                TempData["Error"] = "Bạn không có quyền truy cập chức năng này.";
                return RedirectToAction("Index", "Home");
            }

            var response = await _customerService.DeleteAsync(id);
            
            if (response.Success)
            {
                TempData["SuccessMessage"] = "Xóa khách hàng thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = response.Message;
            }
            
            return RedirectToAction("Index");
        }
    }
}
