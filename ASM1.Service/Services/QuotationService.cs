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

        public QuotationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<IEnumerable<Quotation>>> GetAllAsync()
        {
            try
            {
                var quotations = await _unitOfWork.Quotations.GetAllAsync();
                return ServiceResponse<IEnumerable<Quotation>>.SuccessResponse(quotations);
            }
            catch (Exception ex)
            {
                return ServiceResponse<IEnumerable<Quotation>>.ErrorResponse("Error getting quotations", ex.Message);
            }
        }

        public async Task<ServiceResponse<Quotation?>> GetByIdAsync(int id)
        {
            try
            {
                var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
                return ServiceResponse<Quotation?>.SuccessResponse(quotation);
            }
            catch (Exception ex)
            {
                return ServiceResponse<Quotation?>.ErrorResponse("Error getting quotation", ex.Message);
            }
        }

        public async Task<ServiceResponse<IEnumerable<Quotation>>> GetByCustomerIdAsync(int customerId)
        {
            try
            {
                var quotations = await _unitOfWork.Quotations.GetAllAsync();
                var customerQuotations = quotations.Where(q => q.CustomerId == customerId);
                return ServiceResponse<IEnumerable<Quotation>>.SuccessResponse(customerQuotations);
            }
            catch (Exception ex)
            {
                return ServiceResponse<IEnumerable<Quotation>>.ErrorResponse("Error getting customer quotations", ex.Message);
            }
        }

        public async Task<ServiceResponse> AddAsync(Quotation quotation)
        {
            try
            {
                // Validate required fields
                if (quotation.CustomerId < 0)
                    return ServiceResponse.ErrorResponse("Customer ID is required");
                    
                if (quotation.VariantId <= 0)
                    return ServiceResponse.ErrorResponse("Vehicle variant ID is required");
                    
                if (quotation.DealerId <= 0)
                    return ServiceResponse.ErrorResponse("Dealer ID is required");
                    
                if (quotation.Price <= 0)
                    return ServiceResponse.ErrorResponse("Price must be greater than 0");

                // Generate unique ID if not set
                if (quotation.QuotationId == 0)
                {
                    quotation.QuotationId = await _unitOfWork.Quotations.GenerateUniqueQuotationIdAsync();
                }
                
                // Set default values if not set
                if (quotation.CreatedAt == null)
                    quotation.CreatedAt = DateTime.Now;
                    
                if (string.IsNullOrEmpty(quotation.Status))
                    quotation.Status = "Pending";
                
                await _unitOfWork.Quotations.AddAsync(quotation);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse.SuccessResponse("Quotation added successfully");
            }
            catch (Exception ex)
            {
                // Log the full exception details for debugging
                var innerMessage = ex.InnerException?.Message ?? "";
                var fullMessage = $"{ex.Message}. Inner: {innerMessage}";
                return ServiceResponse.ErrorResponse("Error adding quotation", fullMessage);
            }
        }

        public async Task<ServiceResponse> UpdateAsync(Quotation quotation)
        {
            try
            {
                await _unitOfWork.Quotations.UpdateAsync(quotation);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse.SuccessResponse("Quotation updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.ErrorResponse("Error updating quotation", ex.Message);
            }
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            try
            {
                await _unitOfWork.Quotations.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse.SuccessResponse("Quotation deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.ErrorResponse("Error deleting quotation", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> ApproveAsync(int id)
        {
            try
            {
                var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
                if (quotation == null)
                {
                    return ServiceResponse<bool>.ErrorResponse("Quotation not found");
                }

                quotation.Status = "Approved";
                await _unitOfWork.Quotations.UpdateAsync(quotation);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Quotation approved successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error approving quotation", ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> CancelAsync(int id)
        {
            try
            {
                var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
                if (quotation == null)
                {
                    return ServiceResponse<bool>.ErrorResponse("Quotation not found");
                }

                quotation.Status = "Cancelled";
                await _unitOfWork.Quotations.UpdateAsync(quotation);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true, "Quotation cancelled successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.ErrorResponse("Error cancelling quotation", ex.Message);
            }
        }

        public async Task<ServiceResponse<Quotation?>> GetDetailsByIdAsync(int id)
        {
            try
            {
                var quotation = await _unitOfWork.Quotations.GetByIdAsync(id);
                return ServiceResponse<Quotation?>.SuccessResponse(quotation);
            }
            catch (Exception ex)
            {
                return ServiceResponse<Quotation?>.ErrorResponse("Error getting quotation details", ex.Message);
            }
        }

        public async Task<ServiceResponse<decimal>> CalculatePricingAsync(int variantId, int customerId)
        {
            try
            {
                var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(variantId);
                if (variant == null)
                {
                    return ServiceResponse<decimal>.ErrorResponse("Vehicle variant not found");
                }

                // Simple pricing calculation - just return the variant price
                return ServiceResponse<decimal>.SuccessResponse(variant.Price ?? 0, "Pricing calculated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<decimal>.ErrorResponse("Error calculating pricing", ex.Message);
            }
        }

        public async Task<ServiceResponse<decimal>> CalculatePricingWithPromotionsAsync(int variantId, int customerId, List<string> promotionCodes)
        {
            try
            {
                var variant = await _unitOfWork.VehicleVariants.GetByIdAsync(variantId);
                if (variant == null)
                {
                    return ServiceResponse<decimal>.ErrorResponse("Vehicle variant not found");
                }

                decimal finalPrice = variant.Price ?? 0;
                
                // Apply promotions (simplified logic)
                foreach (var code in promotionCodes)
                {
                    // This is a simplified implementation
                    // In real application, you would look up promotion details and apply discounts
                    finalPrice *= 0.9m; // 10% discount per promotion code
                }

                return ServiceResponse<decimal>.SuccessResponse(finalPrice, "Pricing with promotions calculated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse<decimal>.ErrorResponse("Error calculating pricing with promotions", ex.Message);
            }
        }
    }
}