using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    [Route("[controller]")]
    public class ManagerController : Controller
    {
        private readonly IManagerService _managerService;
        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            ViewBag.TotalCustomers = _managerService.GetTotalCustomers();
            ViewBag.TotalFeedbacks = _managerService.GetTotalFeedbacks();
            ViewBag.TotalTestDrives = _managerService.GetTotalTestDrives();
            ViewBag.Customers = _managerService.GetAllCustomers();
            return View();
        }
        
    }
}
