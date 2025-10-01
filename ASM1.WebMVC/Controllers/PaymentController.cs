using ASM1.Service.Services.Interfaces;
using ASM1.WebMVC.Extensions;
using ASM1.WebMVC.Models;
using ASM1.Repository.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService, IOrderService orderService, IMapper mapper)
        {
            _paymentService = paymentService;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int orderId)
        {
            try
            {
                // Get order details to pre-fill payment form
                var order = await _orderService.GetByIdAsync(orderId);
                if (order == null)
                {
                    TempData["Error"] = "Order not found.";
                    return RedirectToAction("Index", "Order");
                }

                var model = new PaymentCreateViewModel
                {
                    OrderId = orderId,
                    Amount = order.Variant?.Price ?? 0,
                    PaymentDate = DateOnly.FromDateTime(DateTime.Now),
                    Method = "Cash" // Default payment method
                };

                ViewBag.Order = order;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading payment form: {ex.Message}";
                return RedirectToAction("Index", "Order");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var order = await _orderService.GetByIdAsync(model.OrderId);
                    ViewBag.Order = order;
                    return View(model);
                }

                // Set payment date if not provided
                if (!model.PaymentDate.HasValue)
                {
                    model.PaymentDate = DateOnly.FromDateTime(DateTime.Now);
                }

                var payment = _mapper.Map<Payment>(model);
                await _paymentService.AddAsync(payment);
                TempData["Success"] = "Payment has been created successfully!";
                return RedirectToAction("Details", "Order", new { id = model.OrderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error creating payment: {ex.Message}";
                var order = await _orderService.GetByIdAsync(model.OrderId);
                ViewBag.Order = order;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var payments = await _paymentService.GetAllAsync();
                var viewModel = _mapper.Map<List<PaymentViewModel>>(payments);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<PaymentViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var payment = await _paymentService.GetByIdAsync(id);
                if (payment == null)
                {
                    TempData["Error"] = "Payment not found.";
                    return RedirectToAction("Index");
                }

                var viewModel = _mapper.Map<PaymentViewModel>(payment);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _paymentService.DeleteAsync(id);
                TempData["Success"] = "Payment deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting payment: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}