using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _service;

        public AuthController(ILogger<AuthController> logger, IAuthService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _service.Login(email, password);

            if (user == null)
            {
                ViewBag.Error = "Sai email hoặc mật khẩu";
                return View();
            }

            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            // switch (user.Role.ToLower())
            // {
            //     case "admin":
            //         return RedirectToAction("Index", "AdminDashboard");
            //     case "dealer":
            //         return RedirectToAction("Index", "DealerDashboard");
            //     case "staff":
            //         return RedirectToAction("Index", "StaffDashboard");
            //     case "customer":
            //         return RedirectToAction("Index", "CustomerPortal");
            //     default:
            //         return RedirectToAction("Index", "Home");
            // }
            switch (user.Role.ToLower())
            {
                case "admin":
                case "dealer":
                case "staff":
                case "customer":
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(
            string fullName,
            string email,
            string phone,
            string password,
            string confirmPassword
        )
        {
            // Kiểm tra mật khẩu xác nhận
            if (password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp";
                return View();
            }

            // Kiểm tra email đã tồn tại
            var existingUser = await _service.GetUserByEmail(email);
            if (existingUser != null)
            {
                ViewBag.Error = "Email đã được sử dụng";
                return View();
            }

            // Tạo user mới với vai trò mặc định là customer
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Phone = phone,
                Password = password,
                Role = "customer",
            };

            var result = await _service.Register(newUser);
            if (result)
            {
                ViewBag.Success = "Đăng ký thành công! Vui lòng đăng nhập.";
                return View();
            }
            else
            {
                ViewBag.Error = "Đăng ký thất bại. Vui lòng thử lại sau.";
                return View();
            }
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
