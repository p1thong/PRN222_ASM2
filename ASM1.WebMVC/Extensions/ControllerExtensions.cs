using ASM1.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Xử lý ServiceResponse và add errors vào ModelState
        /// </summary>
        public static void HandleResponse(this Controller controller, ServiceResponse response)
        {
            if (!response.Success)
            {
                controller.ModelState.AddModelError(string.Empty, response.Message);
                foreach (var error in response.Errors)
                {
                    controller.ModelState.AddModelError(string.Empty, error);
                }
            }
        }

        /// <summary>
        /// Xử lý ServiceResponse với data và add errors vào ModelState
        /// </summary>
        public static void HandleResponse<T>(this Controller controller, ServiceResponse<T> response)
        {
            if (!response.Success)
            {
                controller.ModelState.AddModelError(string.Empty, response.Message);
                foreach (var error in response.Errors)
                {
                    controller.ModelState.AddModelError(string.Empty, error);
                }
            }
        }

        /// <summary>
        /// Xử lý ServiceResponse và set TempData messages
        /// </summary>
        public static void HandleResponseWithTempData(this Controller controller, ServiceResponse response)
        {
            if (response.Success)
            {
                controller.TempData["Success"] = response.Message;
            }
            else
            {
                controller.TempData["Error"] = response.Message;
                if (response.Errors.Any())
                {
                    controller.TempData["DetailErrors"] = string.Join(", ", response.Errors);
                }
            }
        }

        /// <summary>
        /// Xử lý ServiceResponse với data và set TempData messages
        /// </summary>
        public static void HandleResponseWithTempData<T>(this Controller controller, ServiceResponse<T> response)
        {
            if (response.Success)
            {
                controller.TempData["Success"] = response.Message;
            }
            else
            {
                controller.TempData["Error"] = response.Message;
                if (response.Errors.Any())
                {
                    controller.TempData["DetailErrors"] = string.Join(", ", response.Errors);
                }
            }
        }
    }
}