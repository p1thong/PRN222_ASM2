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

        public async Task AddAsync(QuotationCreateViewModel quotationVm)
        {
            var quotation = _mapper.Map<Quotation>(quotationVm);
            quotation.QuotationId = await _unitOfWork.Quotations.GenerateUniqueIdAsync(q => q.QuotationId);
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
