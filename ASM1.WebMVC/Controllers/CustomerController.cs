using ASM1.Service.Services.Interfaces;
using ASM1.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class CustomerController : Controller
    {
    private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();
            return View(customers);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Lấy DealerId từ session
            if (HttpContext.Session.GetInt32("DealerId") is int dealerId)
            {
                model.DealerId = dealerId;
                await _customerService.AddAsync(model);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, "Không tìm thấy Dealer đang đăng nhập.");
            return View(model);
        }
    }
}
