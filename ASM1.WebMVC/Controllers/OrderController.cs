using ASM1.Service.Services.Interfaces;
using ASM1.WebMVC.Extensions;
using ASM1.WebMVC.Models;
using ASM1.Repository.Models;
using AutoMapper;
using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
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
            try
            {
                var orders = await _orderService.GetAllAsync();
                var orderViewModels = _mapper.Map<List<OrderViewModel>>(orders);
                return View(orderViewModels);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<OrderViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? quotationId)
        {
            try
            {
                var model = new OrderCreateViewModel
                {
                    Status = "Pending",
                    OrderDate = DateOnly.FromDateTime(DateTime.Now)
                };

                if (quotationId.HasValue)
                {
                    var quotation = await _quotationService.GetByIdAsync(quotationId.Value);
                    if (quotation != null)
                    {
                        model.CustomerId = quotation.CustomerId;
                        model.VariantId = quotation.VariantId;
                        model.TotalAmount = quotation.Price;

                        ViewBag.Customer = quotation.Customer;
                        ViewBag.VehicleVariant = quotation.Variant;
                    }
                }

                var customers = await _customerService.GetAllAsync();
                var variants = await _vehicleVariantService.GetAllAsync();

                ViewBag.Customers = _mapper.Map<List<CustomerViewModel>>(customers);
                ViewBag.VehicleVariants = _mapper.Map<List<VehicleVariantViewModel>>(variants);

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi tải trang tạo đơn hàng: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var customers = await _customerService.GetAllAsync();
                    var variants = await _vehicleVariantService.GetAllAsync();

                    ViewBag.Customers = _mapper.Map<List<CustomerViewModel>>(customers);
                    ViewBag.VehicleVariants = _mapper.Map<List<VehicleVariantViewModel>>(variants);
                    return View(model);
                }

                var variant = await _vehicleVariantService.GetByIdAsync(model.VariantId);
                if (variant?.Price.HasValue == true)
                {
                    model.TotalAmount = variant.Price.Value;
                }

                var order = _mapper.Map<Order>(model);
                await _orderService.AddAsync(order);

                TempData["Success"] = "Tạo đơn hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi tạo đơn hàng: " + ex.Message;
                
                var customers = await _customerService.GetAllAsync();
                var variants = await _vehicleVariantService.GetAllAsync();

                ViewBag.Customers = _mapper.Map<List<CustomerViewModel>>(customers);
                ViewBag.VehicleVariants = _mapper.Map<List<VehicleVariantViewModel>>(variants);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng";
                    return RedirectToAction(nameof(Index));
                }

                var orderViewModel = _mapper.Map<OrderViewModel>(order);
                return View(orderViewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi tải chi tiết đơn hàng: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (order == null)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng";
                    return RedirectToAction(nameof(Index));
                }

                var orderViewModel = _mapper.Map<OrderViewModel>(order);
                
                var customers = await _customerService.GetAllAsync();
                var variants = await _vehicleVariantService.GetAllAsync();

                ViewBag.Customers = _mapper.Map<List<CustomerViewModel>>(customers);
                ViewBag.VehicleVariants = _mapper.Map<List<VehicleVariantViewModel>>(variants);

                return View(orderViewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi tải trang chỉnh sửa: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var customers = await _customerService.GetAllAsync();
                    var variants = await _vehicleVariantService.GetAllAsync();

                    ViewBag.Customers = _mapper.Map<List<CustomerViewModel>>(customers);
                    ViewBag.VehicleVariants = _mapper.Map<List<VehicleVariantViewModel>>(variants);
                    return View(model);
                }

                var order = _mapper.Map<Order>(model);
                await _orderService.UpdateAsync(order);

                TempData["Success"] = "Cập nhật đơn hàng thành công!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật đơn hàng: " + ex.Message;
                
                var customers = await _customerService.GetAllAsync();
                var variants = await _vehicleVariantService.GetAllAsync();

                ViewBag.Customers = _mapper.Map<List<CustomerViewModel>>(customers);
                ViewBag.VehicleVariants = _mapper.Map<List<VehicleVariantViewModel>>(variants);
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
    }
}
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
