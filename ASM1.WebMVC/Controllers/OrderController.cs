using ASM1.Service.Services.Interfaces;
using ASM1.WebMVC.Extensions;
using ASM1.WebMVC.Models;
using ASM1.Repository.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IQuotationService _quotationService;
        private readonly IVehicleVariantService _vehicleVariantService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, ICustomerService customerService, 
            IQuotationService quotationService, IVehicleVariantService vehicleVariantService, IMapper mapper)
        {
            _orderService = orderService;
            _customerService = customerService;
            _quotationService = quotationService;
            _vehicleVariantService = vehicleVariantService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllAsync();
            var viewModel = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                viewModel.Add(new OrderViewModel
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    CustomerName = order.Customer?.FullName ?? "Unknown",
                    DealerName = order.Dealer?.FullName ?? "Unknown",
                    VehicleInfo = $"{order.Variant?.VehicleModel?.Name} - {order.Variant?.Version}",
                    TotalAmount = order.Variant?.Price
                });
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var viewModel = order;
            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                var variants = await _vehicleVariantService.GetAllAsync();

                ViewBag.Customers = customers;
                ViewBag.VehicleVariants = variants;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi tải dữ liệu: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var customers = await _customerService.GetAllAsync();
                    var variants = await _vehicleVariantService.GetAllAsync();

                    ViewBag.Customers = customers;
                    ViewBag.VehicleVariants = variants;
                    return View(model);
                }

                model.OrderDate = DateOnly.FromDateTime(DateTime.Now);
                model.Status = "Pending";

                await _orderService.AddAsync(model);

                TempData["Success"] = "Tạo đơn hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi tạo đơn hàng: " + ex.Message;
                
                var customers = await _customerService.GetAllAsync();
                var variants = await _vehicleVariantService.GetAllAsync();

                ViewBag.Customers = customers;
                ViewBag.VehicleVariants = variants;
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                var customers = await _customerService.GetAllAsync();
                var variants = await _vehicleVariantService.GetAllAsync();

                ViewBag.Customers = customers;
                ViewBag.VehicleVariants = variants;

                return View(order);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi tải dữ liệu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order model)
        {
            if (id != model.OrderId)
            {
                return NotFound();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    var customers = await _customerService.GetAllAsync();
                    var variants = await _vehicleVariantService.GetAllAsync();

                    ViewBag.Customers = customers;
                    ViewBag.VehicleVariants = variants;
                    return View(model);
                }

                await _orderService.UpdateAsync(model);

                TempData["Success"] = "Cập nhật đơn hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật đơn hàng: " + ex.Message;
                
                var customers = await _customerService.GetAllAsync();
                var variants = await _vehicleVariantService.GetAllAsync();

                ViewBag.Customers = customers;
                ViewBag.VehicleVariants = variants;
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteAsync(id);
                TempData["Success"] = "Xóa đơn hàng thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi xóa đơn hàng: " + ex.Message;
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