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
            return Ok(_dealerService.GetAllTestDrives());
        }

        [HttpPut("testdrives/{id}/status")]
        public IActionResult UpdateTestDriveStatus(int testDriveId, [FromBody] string status) 
        {
            _dealerService.UpdateTestDriveStatus(testDriveId, status);
            return NoContent();
        }

        [HttpGet("customers")]
        public IActionResult GetAllCustomers()
        {
            return Ok(_dealerService.GetAllCustomers());
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
            return Ok(_dealerService.GetAllFeedbacks());
        }
    }
}
