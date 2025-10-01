using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Add error message to ModelState
        /// </summary>
        public static void AddError(this Controller controller, string errorMessage)
        {
            controller.ModelState.AddModelError(string.Empty, errorMessage);
        }

        /// <summary>
        /// Set success message in TempData
        /// </summary>
        public static void SetSuccessMessage(this Controller controller, string message)
        {
            controller.TempData["Success"] = message;
        }

        /// <summary>
        /// Set error message in TempData
        /// </summary>
        public static void SetErrorMessage(this Controller controller, string message)
        {
            controller.TempData["Error"] = message;
        }
    }
}