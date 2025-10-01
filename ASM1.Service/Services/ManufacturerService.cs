using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IDealerContractRepository _dealerContractRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public ManufacturerService(
            IDealerContractRepository dealerContractRepository,
            IOrderRepository orderRepository,
            IVehicleRepository vehicleRepository)
        {
            _dealerContractRepository = dealerContractRepository;
            _orderRepository = orderRepository;
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Dictionary<string, object>> GetManufacturerDashboardDataAsync(int manufacturerId)
        {
            var totalDealers = await GetTotalDealersByManufacturerAsync(manufacturerId);
            var totalOrders = await GetTotalOrdersByManufacturerAsync(manufacturerId);
            var totalSales = await GetTotalSalesByManufacturerAsync(manufacturerId);
            var totalTargetSales = await GetTotalTargetSalesByManufacturerAsync(manufacturerId);
            var activeContracts = await GetActiveContractsByManufacturerAsync(manufacturerId);
            var topDealers = await GetTopPerformingDealersAsync(manufacturerId, 3);
            var topModels = await GetTopSellingModelsAsync(manufacturerId, 3);

            return new Dictionary<string, object>
            {
                ["TotalDealers"] = totalDealers,
                ["TotalOrders"] = totalOrders,
                ["TotalSales"] = totalSales,
                ["TotalTargetSales"] = totalTargetSales,
                ["SalesAchievement"] = totalTargetSales > 0 ? (totalSales / totalTargetSales) * 100 : 0,
                ["ActiveContracts"] = activeContracts.Count(),
                ["TopDealers"] = topDealers,
                ["TopModels"] = topModels
            };
        }

        public async Task<int> GetTotalDealersByManufacturerAsync(int manufacturerId)
        {
            return await Task.FromResult(
                _dealerContractRepository.GetContractsByManufacturer(manufacturerId).Count()
            );
        }

        public async Task<int> GetTotalOrdersByManufacturerAsync(int manufacturerId)
        {
            // Get all vehicle variants for this manufacturer
            var vehicleModels = await _vehicleRepository.GetVehicleModelsByManufacturerAsync(manufacturerId);
            var variantIds = vehicleModels.SelectMany(vm => vm.VehicleVariants).Select(vv => vv.VariantId);
            
            // Count orders for these variants
            var totalOrders = 0;
            foreach (var variantId in variantIds)
            {
                totalOrders += _orderRepository.GetOrdersByVariant(variantId).Count();
            }
            
            return await Task.FromResult(totalOrders);
        }

        public async Task<decimal> GetTotalSalesByManufacturerAsync(int manufacturerId)
        {
            // Get all vehicle variants for this manufacturer
            var vehicleModels = await _vehicleRepository.GetVehicleModelsByManufacturerAsync(manufacturerId);
            var variantIds = vehicleModels.SelectMany(vm => vm.VehicleVariants).Select(vv => vv.VariantId);
            
            // Calculate total sales for these variants
            decimal totalSales = 0;
            foreach (var variantId in variantIds)
            {
                var orders = _orderRepository.GetOrdersByVariant(variantId);
                foreach (var order in orders)
                {
                    if (order.Variant.Price.HasValue)
                    {
                        totalSales += order.Variant.Price.Value;
                    }
                }
            }
            
            return await Task.FromResult(totalSales);
        }

        public async Task<decimal> GetTotalTargetSalesByManufacturerAsync(int manufacturerId)
        {
            return await Task.FromResult(
                _dealerContractRepository.GetTotalTargetSalesByManufacturer(manufacturerId)
            );
        }

        public async Task<IEnumerable<object>> GetDealerPerformanceAsync(int manufacturerId)
        {
            var contracts = _dealerContractRepository.GetContractsByManufacturer(manufacturerId);
            var performance = new List<object>();

            foreach (var contract in contracts)
            {
                var dealerOrders = _orderRepository.GetOrdersByDealer(contract.DealerId);
                var dealerSales = dealerOrders
                    .Where(o => o.Variant.VehicleModel.ManufacturerId == manufacturerId)
                    .Sum(o => o.Variant.Price ?? 0);

                var achievement = contract.TargetSales > 0 ? (dealerSales / contract.TargetSales.Value) * 100 : 0;

                performance.Add(new
                {
                    DealerId = contract.DealerId,
                    DealerName = contract.Dealer.FullName,
                    TargetSales = contract.TargetSales,
                    ActualSales = dealerSales,
                    Achievement = Math.Round(achievement, 2),
                    CreditLimit = contract.CreditLimit,
                    SignedDate = contract.SignedDate
                });
            }

            return await Task.FromResult(performance);
        }

        public async Task<IEnumerable<object>> GetTopPerformingDealersAsync(int manufacturerId, int topCount = 5)
        {
            var performance = await GetDealerPerformanceAsync(manufacturerId);
            return performance
                .OrderByDescending(p => ((dynamic)p).Achievement)
                .Take(topCount);
        }

        public async Task<IEnumerable<object>> GetUnderperformingDealersAsync(int manufacturerId)
        {
            var performance = await GetDealerPerformanceAsync(manufacturerId);
            return performance
                .Where(p => ((dynamic)p).Achievement < 50) // Less than 50% achievement
                .OrderBy(p => ((dynamic)p).Achievement);
        }

        public async Task<IEnumerable<object>> GetOrderAnalyticsByManufacturerAsync(int manufacturerId)
        {
            var vehicleModels = await _vehicleRepository.GetVehicleModelsByManufacturerAsync(manufacturerId);
            var analytics = new List<object>();

            foreach (var model in vehicleModels)
            {
                var totalOrders = 0;
                var totalSales = 0m;

                foreach (var variant in model.VehicleVariants)
                {
                    var orders = _orderRepository.GetOrdersByVariant(variant.VariantId);
                    totalOrders += orders.Count();
                    totalSales += orders.Sum(o => o.Variant.Price ?? 0);
                }

                analytics.Add(new
                {
                    ModelId = model.VehicleModelId,
                    ModelName = model.Name,
                    Category = model.Category,
                    TotalOrders = totalOrders,
                    TotalSales = totalSales,
                    VariantCount = model.VehicleVariants.Count
                });
            }

            return analytics;
        }

        public async Task<IEnumerable<object>> GetMonthlySalesTrendAsync(int manufacturerId, int months = 12)
        {
            // This is a simplified implementation
            // In a real scenario, you'd need to track order dates and calculate monthly trends
            var totalSales = await GetTotalSalesByManufacturerAsync(manufacturerId);
            var monthlyAverage = totalSales / months;
            
            var trend = new List<object>();
            for (int i = 0; i < months; i++)
            {
                var month = DateTime.Now.AddMonths(-i);
                trend.Add(new
                {
                    Month = month.ToString("yyyy-MM"),
                    Sales = monthlyAverage + (new Random().Next(-10000, 10000)) // Simulated variation
                });
            }

            return trend.Reverse();
        }

        public async Task<IEnumerable<DealerContract>> GetActiveContractsByManufacturerAsync(int manufacturerId)
        {
            return await Task.FromResult(
                _dealerContractRepository.GetContractsByManufacturer(manufacturerId)
                    .Where(c => c.SignedDate.HasValue)
            );
        }

        public async Task<IEnumerable<DealerContract>> GetExpiringContractsAsync(int manufacturerId, int daysAhead = 30)
        {
            var contracts = await GetActiveContractsByManufacturerAsync(manufacturerId);
            var expiryDate = DateOnly.FromDateTime(DateTime.Now.AddDays(daysAhead));
            
            // Note: This is simplified - in reality, contracts might have expiry dates
            return contracts.Take(2); // Return first 2 as example of "expiring soon"
        }

        public async Task<IEnumerable<object>> GetVehicleModelPerformanceAsync(int manufacturerId)
        {
            return await GetOrderAnalyticsByManufacturerAsync(manufacturerId);
        }

        public async Task<IEnumerable<object>> GetTopSellingModelsAsync(int manufacturerId, int topCount = 5)
        {
            var performance = await GetVehicleModelPerformanceAsync(manufacturerId);
            return performance
                .OrderByDescending(p => ((dynamic)p).TotalSales)
                .Take(topCount);
        }
    }
}
