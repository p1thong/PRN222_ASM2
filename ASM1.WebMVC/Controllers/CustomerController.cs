using ASM1.Service.Services.Interfaces;
using ASM1.Service.Models;
using ASM1.WebMVC.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IQuotationService _quotationService;

        public CustomerController(ICustomerService customerService, IQuotationService quotationService)
        {
            _customerService = customerService;
            _quotationService = quotationService;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _customerService.GetAllAsync();
            
            if (!response.Success)
            {
                this.HandleResponseWithTempData(response);
                return View(new List<CustomerViewModel>());
            }
            
            return View(response.Data ?? new List<CustomerViewModel>());
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
                
                return HandleServiceResponse(response, "Index", "Add", model);
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
                this.HandleResponseWithTempData(response);
                return RedirectToAction("Index");
            }

            // Lấy danh sách quotations cho customer này
            var quotations = await _quotationService.GetByCustomerIdAsync(id);
            ViewBag.Quotations = quotations;

            return View(response.Data);
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
                this.HandleResponseWithTempData(response);
                return RedirectToAction("Index");
            }

            return View(response.Data);
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

            var response = await _customerService.UpdateAsync(model);
            return HandleServiceResponse(response, "Index", "Update", model);
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
            this.HandleResponseWithTempData(response);
            
            return RedirectToAction("Index");
﻿using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService) 
        {
            _customerService = customerService;
        }

        [HttpGet("schedule")]
        public IActionResult ScheduleTestDrive()
        {
            return View();
        }

        [HttpPost("schedule")]
        [ValidateAntiForgeryToken]
        public IActionResult ScheduleTestDrive([FromBody] TestDrive testDrive)
        {
            if (ModelState.IsValid) 
            {
                _customerService.ScheduleTestDrive(testDrive);
                return RedirectToAction("ScheduleSuccess");
            }

            return View(testDrive);
        }

        [HttpGet("schedule-success")]
        public IActionResult ScheduleSuccess()
        {
            return View();
        }

        [HttpGet("feedback")]
        public IActionResult SendFeedback()
        {
            return View(); 
        }

        [HttpPost("feedback")]
        public IActionResult SendFeedback([FromBody] Feedback feedback) 
        {
            if (ModelState.IsValid)
            {
                _customerService.SendFeedback(feedback);
                return RedirectToAction("FeedbackSuccess");
            }
            return View(feedback);
        }

        [HttpGet("feedback-success")]
        public IActionResult FeedbackSuccess()
        {
            return View();
        }
    }
}
