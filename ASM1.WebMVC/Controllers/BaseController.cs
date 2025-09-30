using ASM1.WebMVC.Models;
using ASM1.WebMVC.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Xử lý ServiceResponse và trả về view với model state được cập nhật
        /// </summary>
        protected IActionResult HandleServiceResponse<T>(ServiceResponse<T> response, string viewName, object model)
        {
            if (!response.Success)
            {
                this.HandleResponse(response);
                return View(viewName, model);
            }

            return View(viewName, response.Data);
        }

        /// <summary>
        /// Xử lý ServiceResponse và redirect hoặc return view
        /// </summary>
        protected IActionResult HandleServiceResponse(ServiceResponse response, string successRedirectAction, string errorViewName, object model)
        {
            if (response.Success)
            {
                TempData["Success"] = response.Message;
                return RedirectToAction(successRedirectAction);
            }
            else
            {
                this.HandleResponse(response);
                return View(errorViewName, model);
            }
        }

        /// <summary>
        /// Xử lý ServiceResponse và redirect hoặc return view với controller khác
        /// </summary>
        protected IActionResult HandleServiceResponse(ServiceResponse response, string successRedirectAction, string successRedirectController, string errorViewName, object model)
        {
            if (response.Success)
            {
                TempData["Success"] = response.Message;
                return RedirectToAction(successRedirectAction, successRedirectController);
            }
            else
            {
                this.HandleResponse(response);
                return View(errorViewName, model);
            }
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