using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Models;
using ASM1.Service.Services.Interfaces;
using AutoMapper;

namespace ASM1.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentViewModel>> GetAllAsync()
        {
            var payments = await _unitOfWork.Payments.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentViewModel>>(payments);
        }

        public async Task<PaymentViewModel?> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            return payment == null ? null : _mapper.Map<PaymentViewModel>(payment);
        }

        public async Task AddAsync(PaymentCreateViewModel paymentVm)
        {
            var payment = _mapper.Map<Payment>(paymentVm);
            payment.PaymentId = await _unitOfWork.Payments.GenerateUniqueIdAsync(p => p.PaymentId);
            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(PaymentViewModel paymentVm)
        {
            var payment = _mapper.Map<Payment>(paymentVm);
            await _unitOfWork.Payments.UpdateAsync(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Payments.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
