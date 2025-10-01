using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class PromotionController : Controller
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        // GET: Promotion
        public async Task<IActionResult> Index()
        {
            var promotions = await _promotionService.GetAllPromotionsAsync();
            return View(promotions);
        }

        // GET: Promotion/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var promotion = await _promotionService.GetPromotionByIdAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }

        // GET: Promotion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Promotion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                var result = await _promotionService.CreatePromotionAsync(promotion);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Promotion created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Failed to create promotion. Please check your input.");
            }
            return View(promotion);
        }

        // GET: Promotion/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var promotion = await _promotionService.GetPromotionByIdAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }

        // POST: Promotion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Promotion promotion)
        {
            if (id != promotion.PromotionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _promotionService.UpdatePromotionAsync(promotion);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Promotion updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Failed to update promotion.");
            }
            return View(promotion);
        }

        // GET: Promotion/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var promotion = await _promotionService.GetPromotionByIdAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }

        // POST: Promotion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _promotionService.DeletePromotionAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Promotion deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete promotion.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Promotion/ByOrder/5
        public async Task<IActionResult> ByOrder(int orderId)
        {
            var promotions = await _promotionService.GetPromotionsByOrderAsync(orderId);
            ViewBag.OrderId = orderId;
            return View(promotions);
        }

        // GET: Promotion/Active
        public async Task<IActionResult> Active()
        {
            var promotions = await _promotionService.GetActivePromotionsAsync();
            return View(promotions);
        }

        // GET: Promotion/Expired
        public async Task<IActionResult> Expired()
        {
            var promotions = await _promotionService.GetExpiredPromotionsAsync();
            return View(promotions);
        }

        // GET: Promotion/ByCode/SUMMER2024
        public async Task<IActionResult> ByCode(string promotionCode)
        {
            var promotions = await _promotionService.GetPromotionsByCodeAsync(promotionCode);
            ViewBag.PromotionCode = promotionCode;
            return View(promotions);
        }

        // POST: Promotion/ApplyToOrder
        [HttpPost]
        public async Task<IActionResult> ApplyToOrder(int orderId, string promotionCode)
        {
            var canApply = await _promotionService.CanApplyPromotionAsync(orderId, promotionCode);
            if (!canApply)
            {
                TempData["ErrorMessage"] = "Cannot apply this promotion to the order.";
                return RedirectToAction("Details", "Order", new { id = orderId });
            }

            var discount = await _promotionService.CalculateDiscountAsync(orderId, promotionCode);
            var promotion = new Promotion
            {
                OrderId = orderId,
                PromotionCode = promotionCode,
                DiscountAmount = discount,
                ValidUntil = DateOnly.FromDateTime(DateTime.Now.AddDays(30)) // Default 30 days
            };

            var result = await _promotionService.CreatePromotionAsync(promotion);
            if (result != null)
            {
                TempData["SuccessMessage"] = $"Promotion {promotionCode} applied successfully! Discount: ${discount:F2}";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to apply promotion.";
            }

            return RedirectToAction("Details", "Order", new { id = orderId });
        }

        // GET: Promotion/ValidateCode/SUMMER2024
        [HttpGet]
        public async Task<IActionResult> ValidateCode(string promotionCode)
        {
            var isValid = await _promotionService.IsPromotionCodeValidAsync(promotionCode);
            return Json(new { isValid = isValid });
        }
    }
}
