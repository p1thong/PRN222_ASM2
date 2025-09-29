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
            var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
            if (quotation == null) return null;

            var variant = quotation.Variant;
            var basePrice = variant.Price ?? 0;
            
            // For existing quotations, we'll extract pricing components from the stored Price
            // This is a simplified approach - in reality, you might want to store these components separately
            var finalPrice = quotation.Price;
            
            // For demonstration, assume some default breakdown
            var estimatedDiscount = 0m; // No default discount
            var estimatedFees = basePrice * 0.02m; // Assume 2% additional fees
            var estimatedTaxRate = 0.1m; // 10% tax
            var estimatedTax = (basePrice - estimatedDiscount + estimatedFees) * estimatedTaxRate;

            var quotationDetail = new QuotationDetailViewModel
            {
                QuotationId = quotation.QuotationId,
                CustomerName = quotation.Customer.FullName,
                CustomerEmail = quotation.Customer.Email,
                CustomerPhone = quotation.Customer.Phone,
                VehicleBrand = quotation.Variant.VehicleModel.Manufacturer.Name,
                VehicleModel = quotation.Variant.VehicleModel.Name,
                VehicleVersion = quotation.Variant.Version,
                VehicleColor = quotation.Variant.Color,
                VehicleYear = quotation.Variant.ProductYear,
                VehicleBasePrice = basePrice,
                DiscountAmount = estimatedDiscount,
                AdditionalFees = estimatedFees,
                TaxRate = estimatedTaxRate,
                DiscountDescription = "Khuyến mãi tháng",
                FeesDescription = "Phí đăng ký, bảo hiểm",
                CreatedAt = quotation.CreatedAt,
                Status = quotation.Status,
                DealerName = quotation.Dealer.FullName
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
            var quotation = _mapper.Map<Quotation>(quotationVm);
            await _unitOfWork.Quotations.UpdateAsync(quotation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Quotations.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
