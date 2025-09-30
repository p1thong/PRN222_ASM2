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

        [HttpGet("total-customer")]
        public IActionResult GetTotalCustomer()
        {
            var total = _managerService.GetTotalCustomers();
            return Ok(total);
        }

        [HttpGet("total-feedbacks")]
        public IActionResult GetTotalFeedbacks()
        {
            var total = _managerService.GetTotalFeedbacks();
            return Ok(total);
        }

        [HttpGet("total-testdrives")]
        public IActionResult GetTotalTestDrives()
        {
            var total = _managerService.GetTotalTestDrives();
            return Ok(total);
        }

        [HttpGet("customers")]
        public IActionResult GetAllCustomers()
        {
            var customers = _managerService.GetAllCustomers();
            return Ok(customers);
        }
    }
}
