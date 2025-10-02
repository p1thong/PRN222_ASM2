using ASM1.Service.Services.Interfaces;
using ASM1.WebMVC.Extensions;
using ASM1.WebMVC.Models;
using ASM1.Repository.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class SalesContractController : BaseController
    {
        private readonly ISalesContractService _salesContractService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public SalesContractController(ISalesContractService salesContractService, IOrderService orderService, IMapper mapper)
        {
            _salesContractService = salesContractService;
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int orderId)
        {
            try
            {
                // Get order details to pre-fill contract form
                var order = await _orderService.GetByIdAsync(orderId);
                if (order == null)
                {
                    TempData["Error"] = "Order not found.";
                    return RedirectToAction("Index", "Order");
                }

                var model = new SalesContractCreateViewModel
                {
                    OrderId = orderId,
                    SignedDate = DateOnly.FromDateTime(DateTime.Now)
                };

                ViewBag.Order = order;
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading sales contract form: {ex.Message}";
                return RedirectToAction("Index", "Order");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesContractCreateViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var order = await _orderService.GetByIdAsync(model.OrderId);
                    ViewBag.Order = order;
                    return View(model);
                }

                // Set signed date if not provided
                if (!model.SignedDate.HasValue)
                {
                    model.SignedDate = DateOnly.FromDateTime(DateTime.Now);
                }

                var contract = _mapper.Map<SalesContract>(model);
                await _salesContractService.AddAsync(contract);
                TempData["Success"] = "Sales contract has been created successfully!";
                return RedirectToAction("Details", "Order", new { id = model.OrderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error creating sales contract: {ex.Message}";
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
                var contracts = await _salesContractService.GetAllAsync();
                var viewModel = _mapper.Map<List<SalesContractViewModel>>(contracts);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<SalesContractViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var contract = await _salesContractService.GetByIdAsync(id);
                if (contract == null)
                {
                    TempData["Error"] = "Sales contract not found.";
                    return RedirectToAction("Index");
                }

                var viewModel = _mapper.Map<SalesContractViewModel>(contract);
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
                await _salesContractService.DeleteAsync(id);
                TempData["Success"] = "Sales contract deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting sales contract: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}