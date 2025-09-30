using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    [Route("[controller]")]
    public class DealerController : Controller
    {
        private readonly IDealerService _dealerService;
        public DealerController(IDealerService dealerService)
        {
            _dealerService = dealerService;
        }

        [HttpGet("testdrives")]
        public IActionResult GetAllTestDrives()
        {
            var testDrives = _dealerService.GetAllTestDrives();
            return View(testDrives);
        }

        [HttpPut("testdrives/{id}/status")]
        public IActionResult UpdateTestDriveStatus(int testDriveId, [FromBody] string status) 
        {
            _dealerService.UpdateTestDriveStatus(testDriveId, status);
            return RedirectToAction("TestDrives");
        }

        [HttpGet("customers")]
        public IActionResult GetAllCustomers()
        {
            var customers = _dealerService.GetAllCustomers();
            return View(customers);
        }

        [HttpPost("customers")]
        public IActionResult SaveCustomer([FromBody] Customer customer)
        {
            _dealerService.SaveCustomerProfile(customer);
            return Ok(customer);
        }

        [HttpGet("feedbacks")]
        public IActionResult GetAllFeedback()
        {
            var feedbacks = _dealerService.GetAllFeedbacks();
            return View(feedbacks);
        }
    }
}
