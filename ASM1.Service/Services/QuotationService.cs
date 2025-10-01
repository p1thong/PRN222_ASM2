using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuotationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Quotation>> GetAllAsync()
        {
            return await _unitOfWork.Quotations.GetAllAsync();
        }

        public async Task<Quotation?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Quotations.GetQuotationWithDetailsAsync(id);
        }

        public async Task<IEnumerable<Quotation>> GetByCustomerIdAsync(int customerId)
        {
            var quotations = await _unitOfWork.Quotations.GetAllAsync();
            return quotations.Where(q => q.CustomerId == customerId).ToList();
        }

        public async Task AddAsync(Quotation quotation)
        {
            quotation.QuotationId = await _unitOfWork.Quotations.GenerateUniqueQuotationIdAsync();
            await _unitOfWork.Quotations.AddAsync(quotation);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Quotation quotation)
        {
            var existingQuotation = await _unitOfWork.Quotations.GetByIdAsync(quotation.QuotationId);
            if (existingQuotation == null)
            {
                throw new InvalidOperationException($"Quotation with ID {quotation.QuotationId} not found.");
            }

            existingQuotation.CustomerId = quotation.CustomerId;
            existingQuotation.VariantId = quotation.VariantId;
            existingQuotation.DealerId = quotation.DealerId;
            existingQuotation.Price = quotation.Price;
            existingQuotation.Status = quotation.Status;

            await _unitOfWork.Quotations.UpdateAsync(existingQuotation);
            await _unitOfWork.SaveChangesAsync();
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
