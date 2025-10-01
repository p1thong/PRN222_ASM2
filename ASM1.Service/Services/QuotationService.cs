using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Models;
using ASM1.Service.Services.Interfaces;
using AutoMapper;

namespace ASM1.Service.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPromotionRuleService _promotionRuleService;

        public QuotationService(IUnitOfWork unitOfWork, IMapper mapper, IPromotionRuleService promotionRuleService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _promotionRuleService = promotionRuleService;
        }

        public async Task<IEnumerable<QuotationViewModel>> GetAllAsync()
        {
            var quotations = await _unitOfWork.Quotations.GetAllAsync();
            return _mapper.Map<IEnumerable<QuotationViewModel>>(quotations);
        }

        public async Task<QuotationViewModel?> GetByIdAsync(int id)
        {
            var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
            return quotation == null ? null : _mapper.Map<QuotationViewModel>(quotation);
        }

        public async Task<IEnumerable<QuotationViewModel>> GetByCustomerIdAsync(int customerId)
        {
            var quotations = await _unitOfWork.Quotations.GetAllAsync();
            var customerQuotations = quotations.Where(q => q.CustomerId == customerId);
            return _mapper.Map<IEnumerable<QuotationViewModel>>(customerQuotations);
        }

        public async Task<QuotationDetailViewModel?> GetDetailsByIdAsync(int id)
        {
            var quotation = await _unitOfWork.Quotations.GetQuotationWithDetailsAsync(id);
            if (quotation == null) return null;

            // Debug: Check what we got
            Console.WriteLine($"Quotation ID: {quotation.QuotationId}");
            Console.WriteLine($"Customer is null: {quotation.Customer == null}");
            Console.WriteLine($"Dealer is null: {quotation.Dealer == null}");
            Console.WriteLine($"Variant is null: {quotation.Variant == null}");

            var variant = quotation.Variant;
            var basePrice = variant?.Price ?? 0;
            
            // For existing quotations, we'll extract pricing components from the stored Price
            // This is a simplified approach - in reality, you might want to store these components separately
            var finalPrice = quotation.Price;
            
            // For demonstration, assume some default breakdown
            var estimatedDiscount = 0m; // No default discount
            var estimatedFees = basePrice * 0.02m; // Assume 2% additional fees
            var estimatedTaxRate = 0.1m; // 10% tax
            var estimatedTax = (basePrice - estimatedDiscount + estimatedFees) * estimatedTaxRate;
            
            // Try to determine actual discount by checking promotion service
            var discountDescription = "";
            var feesDescription = "";
            
            try
            {
                var promotionRequest = new PromotionCalculationRequest
                {
                    VariantId = quotation.VariantId,
                    CustomerId = quotation.CustomerId,
                    BasePrice = basePrice,
                    QuotationDate = quotation.CreatedAt ?? DateTime.Now
                };
                
                var promotionResult = await _promotionRuleService.CalculateApplicablePromotionsAsync(promotionRequest);
                if (promotionResult.TotalDiscount > 0)
                {
                    estimatedDiscount = promotionResult.TotalDiscount;
                    discountDescription = promotionResult.DiscountDescription;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating promotion: {ex.Message}");
            }
            
            // Set fees description only if there are actual fees
            if (estimatedFees > 0)
            {
                feesDescription = "Phí đăng ký, bảo hiểm";
            }

            var quotationDetail = new QuotationDetailViewModel
            {
                QuotationId = quotation.QuotationId,
                CustomerName = quotation.Customer?.FullName ?? "Unknown Customer",
                CustomerEmail = quotation.Customer?.Email ?? "No Email",
                CustomerPhone = quotation.Customer?.Phone ?? "No Phone",
                VehicleBrand = quotation.Variant?.VehicleModel?.Manufacturer?.Name ?? "Unknown Brand",
                VehicleModel = quotation.Variant?.VehicleModel?.Name ?? "Unknown Model",
                VehicleVersion = quotation.Variant?.Version ?? "Unknown Version",
                VehicleColor = quotation.Variant?.Color ?? "Unknown Color",
                VehicleYear = quotation.Variant?.ProductYear,
                VehicleBasePrice = basePrice,
                DiscountAmount = estimatedDiscount,
                AdditionalFees = estimatedFees,
                TaxRate = estimatedTaxRate,
                DiscountDescription = discountDescription,
                FeesDescription = feesDescription,
                CreatedAt = quotation.CreatedAt,
                Status = quotation.Status ?? "Unknown",
                DealerName = quotation.Dealer?.FullName ?? "Unknown Dealer"
            };

            return quotationDetail;
        }

        public async Task<QuotationDetailViewModel> CalculatePricingAsync(QuotationPricingRequest request)
        {
            var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(request.VariantId);
            if (variant?.Price == null)
                throw new InvalidOperationException("Vehicle variant not found or price not set");

            var basePrice = variant.Price.Value;
            var taxAmount = (basePrice - request.DiscountAmount + request.AdditionalFees) * request.TaxRate;
            var finalPrice = basePrice - request.DiscountAmount + request.AdditionalFees + taxAmount;

            return new QuotationDetailViewModel
            {
                VehicleBrand = variant.VehicleModel.Manufacturer.Name,
                VehicleModel = variant.VehicleModel.Name,
                VehicleVersion = variant.Version,
                VehicleColor = variant.Color,
                VehicleYear = variant.ProductYear,
                VehicleBasePrice = basePrice,
                DiscountAmount = request.DiscountAmount,
                AdditionalFees = request.AdditionalFees,
                TaxRate = request.TaxRate,
                DiscountDescription = request.DiscountDescription,
                FeesDescription = request.FeesDescription
            };
        }

        public async Task<QuotationDetailViewModel> CalculatePricingWithPromotionsAsync(int variantId, int customerId, decimal additionalFees = 0, decimal taxRate = 0.1m)
        {
            var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(variantId);
            if (variant?.Price == null)
                throw new InvalidOperationException("Vehicle variant not found or price not set");

            var basePrice = variant.Price.Value;

            // Calculate applicable promotions automatically
            var promotionRequest = new PromotionCalculationRequest
            {
                VariantId = variantId,
                CustomerId = customerId,
                BasePrice = basePrice,
                QuotationDate = DateTime.Now
            };

            var promotionResult = await _promotionRuleService.CalculateApplicablePromotionsAsync(promotionRequest);
            var totalDiscount = promotionResult.TotalDiscount;

            var taxAmount = (basePrice - totalDiscount + additionalFees) * taxRate;
            var finalPrice = basePrice - totalDiscount + additionalFees + taxAmount;

            return new QuotationDetailViewModel
            {
                VehicleBrand = variant.VehicleModel.Manufacturer.Name,
                VehicleModel = variant.VehicleModel.Name,
                VehicleVersion = variant.Version,
                VehicleColor = variant.Color,
                VehicleYear = variant.ProductYear,
                VehicleBasePrice = basePrice,
                DiscountAmount = totalDiscount,
                AdditionalFees = additionalFees,
                TaxRate = taxRate,
                DiscountDescription = promotionResult.DiscountDescription,
                FeesDescription = "Phí đăng ký, bảo hiểm"
            };
        }

        public async Task AddAsync(QuotationCreateViewModel quotationVm)
        {
            // Get the base price from vehicle variant
            var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(quotationVm.VariantId);
            if (variant?.Price == null)
                throw new InvalidOperationException("Vehicle variant not found or price not set");

            // If base price is not set, use variant price
            if (quotationVm.BasePrice == 0)
                quotationVm.BasePrice = variant.Price.Value;

            var quotation = _mapper.Map<Quotation>(quotationVm);
            quotation.QuotationId = await _unitOfWork.Quotations.GenerateUniqueQuotationIdAsync();
            
            // Calculate and store the final price in the Price field
            quotation.Price = quotationVm.FinalPrice;

            await _unitOfWork.Quotations.AddAsync(quotation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(QuotationViewModel quotationVm)
        {
            try
            {
                var existingQuotation = await _unitOfWork.Quotations.GetByIdAsync(quotationVm.QuotationId);
                if (existingQuotation == null)
                {
                    throw new InvalidOperationException($"Quotation with ID {quotationVm.QuotationId} not found.");
                }

                Console.WriteLine($"Found existing quotation: {existingQuotation.QuotationId}");

                // Validate foreign keys
                var customer = await _unitOfWork.Customers.GetByIdAsync(quotationVm.CustomerId);
                if (customer == null)
                {
                    throw new InvalidOperationException($"Customer with ID {quotationVm.CustomerId} not found.");
                }

                var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(quotationVm.VariantId);
                if (variant == null)
                {
                    throw new InvalidOperationException($"Vehicle variant with ID {quotationVm.VariantId} not found.");
                }

                Console.WriteLine($"All foreign keys validated successfully");

                // Update only the properties that can be changed
                existingQuotation.CustomerId = quotationVm.CustomerId;
                existingQuotation.VariantId = quotationVm.VariantId;
                existingQuotation.DealerId = quotationVm.DealerId;
                existingQuotation.Price = quotationVm.Price;
                existingQuotation.Status = quotationVm.Status;
                // CreatedAt should not be updated, keep the original value

                Console.WriteLine($"About to call UpdateAsync on repository...");
                await _unitOfWork.Quotations.UpdateAsync(existingQuotation);
                
                Console.WriteLine($"About to save changes...");
                await _unitOfWork.SaveChangesAsync();
                
                Console.WriteLine($"Successfully updated quotation {quotationVm.QuotationId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Quotations.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ApproveAsync(int id)
        {
            try
            {
                var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
                if (quotation == null)
                    return false;

                quotation.Status = "Approved";
                await _unitOfWork.Quotations.UpdateAsync(quotation);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelAsync(int id)
        {
            try
            {
                var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
                if (quotation == null)
                    return false;

                quotation.Status = "Cancelled";
                await _unitOfWork.Quotations.UpdateAsync(quotation);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
