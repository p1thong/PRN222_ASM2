using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Controllers
{
    public class ManufacturerDashboardController : Controller
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerDashboardController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        // GET: ManufacturerDashboard
        public async Task<IActionResult> Index(int manufacturerId = 1) // Default to manufacturer 1
        {
            var dashboardData = await _manufacturerService.GetManufacturerDashboardDataAsync(manufacturerId);
            ViewBag.ManufacturerId = manufacturerId;
            return View(dashboardData);
        }

        // GET: ManufacturerDashboard/DealerPerformance/1
        public async Task<IActionResult> DealerPerformance(int manufacturerId)
        {
            var performance = await _manufacturerService.GetDealerPerformanceAsync(manufacturerId);
            ViewBag.ManufacturerId = manufacturerId;
            return View(performance);
        }

        // GET: ManufacturerDashboard/TopDealers/1
        public async Task<IActionResult> TopDealers(int manufacturerId, int topCount = 5)
        {
            var topDealers = await _manufacturerService.GetTopPerformingDealersAsync(manufacturerId, topCount);
            ViewBag.ManufacturerId = manufacturerId;
            ViewBag.TopCount = topCount;
            return View(topDealers);
        }

        // GET: ManufacturerDashboard/UnderperformingDealers/1
        public async Task<IActionResult> UnderperformingDealers(int manufacturerId)
        {
            var underperformingDealers = await _manufacturerService.GetUnderperformingDealersAsync(manufacturerId);
            ViewBag.ManufacturerId = manufacturerId;
            return View(underperformingDealers);
        }

        // GET: ManufacturerDashboard/OrderAnalytics/1
        public async Task<IActionResult> OrderAnalytics(int manufacturerId)
        {
            var analytics = await _manufacturerService.GetOrderAnalyticsByManufacturerAsync(manufacturerId);
            ViewBag.ManufacturerId = manufacturerId;
            return View(analytics);
        }

        // GET: ManufacturerDashboard/SalesTrend/1
        public async Task<IActionResult> SalesTrend(int manufacturerId, int months = 12)
        {
            var trend = await _manufacturerService.GetMonthlySalesTrendAsync(manufacturerId, months);
            ViewBag.ManufacturerId = manufacturerId;
            ViewBag.Months = months;
            return View(trend);
        }

        // GET: ManufacturerDashboard/ActiveContracts/1
        public async Task<IActionResult> ActiveContracts(int manufacturerId)
        {
            var contracts = await _manufacturerService.GetActiveContractsByManufacturerAsync(manufacturerId);
            ViewBag.ManufacturerId = manufacturerId;
            return View(contracts);
        }

        // GET: ManufacturerDashboard/ExpiringContracts/1
        public async Task<IActionResult> ExpiringContracts(int manufacturerId, int daysAhead = 30)
        {
            var contracts = await _manufacturerService.GetExpiringContractsAsync(manufacturerId, daysAhead);
            ViewBag.ManufacturerId = manufacturerId;
            ViewBag.DaysAhead = daysAhead;
            return View(contracts);
        }

        // GET: ManufacturerDashboard/ModelPerformance/1
        public async Task<IActionResult> ModelPerformance(int manufacturerId)
        {
            var performance = await _manufacturerService.GetVehicleModelPerformanceAsync(manufacturerId);
            ViewBag.ManufacturerId = manufacturerId;
            return View(performance);
        }

        // GET: ManufacturerDashboard/TopModels/1
        public async Task<IActionResult> TopModels(int manufacturerId, int topCount = 5)
        {
            var topModels = await _manufacturerService.GetTopSellingModelsAsync(manufacturerId, topCount);
            ViewBag.ManufacturerId = manufacturerId;
            ViewBag.TopCount = topCount;
            return View(topModels);
        }

        // GET: ManufacturerDashboard/Summary/1
        public async Task<IActionResult> Summary(int manufacturerId)
        {
            var dashboardData = await _manufacturerService.GetManufacturerDashboardDataAsync(manufacturerId);
            var topDealers = await _manufacturerService.GetTopPerformingDealersAsync(manufacturerId, 3);
            var topModels = await _manufacturerService.GetTopSellingModelsAsync(manufacturerId, 3);
            var underperformingDealers = await _manufacturerService.GetUnderperformingDealersAsync(manufacturerId);

            ViewBag.ManufacturerId = manufacturerId;
            ViewBag.DashboardData = dashboardData;
            ViewBag.TopDealers = topDealers;
            ViewBag.TopModels = topModels;
            ViewBag.UnderperformingDealers = underperformingDealers;

            return View();
        }
    }
}
