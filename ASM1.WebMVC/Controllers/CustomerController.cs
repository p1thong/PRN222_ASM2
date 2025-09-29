using ASM1.Repository.Models;
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

        [HttpPost("schedule")]
        public IActionResult ScheduleTestDrive([FromBody] TestDrive testDrive)
        {
            _customerService.ScheduleTestDrive(testDrive);
            return Ok(testDrive);
        }

        [HttpPost("feedback")]
        public IActionResult SendFeedback([FromBody] Feedback feedback) 
        {
            _customerService.SendFeedback(feedback);
            return Ok(feedback);
        }
    }
}
