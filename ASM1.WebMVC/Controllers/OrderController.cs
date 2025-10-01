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