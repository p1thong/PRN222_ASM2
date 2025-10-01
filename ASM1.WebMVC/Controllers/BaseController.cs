using ASM1.WebMVC.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Handle success operation with redirect
        /// </summary>
        protected IActionResult HandleSuccess(string message, string redirectAction)
        {
            this.SetSuccessMessage(message);
            return RedirectToAction(redirectAction);
        }

        /// <summary>
        /// Handle success operation with redirect to different controller
        /// </summary>
        protected IActionResult HandleSuccess(string message, string redirectAction, string redirectController)
        {
            this.SetSuccessMessage(message);
            return RedirectToAction(redirectAction, redirectController);
        }

        /// <summary>
        /// Handle error operation and return view
        /// </summary>
        protected IActionResult HandleError(string errorMessage, string viewName, object model)
        {
            this.AddError(errorMessage);
            return View(viewName, model);
        }

        /// <summary>
        /// Kiểm tra DealerId từ session
        /// </summary>
        protected int? GetDealerIdFromSession()
        {
            return HttpContext.Session.GetInt32("DealerId");
        }

        /// <summary>
        /// Kiểm tra UserId từ session
        /// </summary>
        protected string? GetUserIdFromSession()
        {
            return HttpContext.Session.GetString("UserId");
        }

        /// <summary>
        /// Kiểm tra UserRole từ session
        /// </summary>
        protected string? GetUserRoleFromSession()
        {
            return HttpContext.Session.GetString("UserRole");
        }

        /// <summary>
        /// Kiểm tra user có đăng nhập không
        /// </summary>
        protected bool IsUserLoggedIn()
        {
            return !string.IsNullOrEmpty(GetUserIdFromSession());
        }

        /// <summary>
        /// Kiểm tra user có phải dealer không
        /// </summary>
        protected bool IsDealerUser()
        {
            var role = GetUserRoleFromSession();
            var lowerRole = role?.ToLower();
            return lowerRole == "dealer" || 
                   lowerRole == "staff" || 
                   lowerRole == "dealerstaff" || 
                   lowerRole == "dealermanager";
        }
    }
}