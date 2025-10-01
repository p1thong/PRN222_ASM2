using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderService.CreateOrderAsync(order);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Order created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Failed to create order. Please check your input.");
            }
            return View(order);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _orderService.UpdateOrderAsync(order);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Order updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                
                ModelState.AddModelError("", "Failed to update order.");
            }
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Order deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete order.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/ByDealer/5
        public async Task<IActionResult> ByDealer(int dealerId)
        {
            var orders = await _orderService.GetOrdersByDealerAsync(dealerId);
            ViewBag.DealerId = dealerId;
            return View(orders);
        }

        // GET: Order/ByCustomer/5
        public async Task<IActionResult> ByCustomer(int customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerAsync(customerId);
            ViewBag.CustomerId = customerId;
            return View(orders);
        }

        // GET: Order/ByStatus/Pending
        public async Task<IActionResult> ByStatus(string status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
            ViewBag.Status = status;
            return View(orders);
        }

        // POST: Order/UpdateStatus/5
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, string status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);
            if (result)
            {
                TempData["SuccessMessage"] = "Order status updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update order status.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Order/Pending
        public async Task<IActionResult> Pending()
        {
            var orders = await _orderService.GetPendingOrdersAsync();
            return View(orders);
        }

        // GET: Order/Completed
        public async Task<IActionResult> Completed()
        {
            var orders = await _orderService.GetCompletedOrdersAsync();
            return View(orders);
        }
    }
}
