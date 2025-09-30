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
