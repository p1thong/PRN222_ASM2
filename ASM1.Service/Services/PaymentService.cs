using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _unitOfWork.Payments.GetAllAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Payments.GetByIdAsync(id);
        }

        public async Task AddAsync(Payment payment)
        {
            payment.PaymentId = await _unitOfWork.Payments.GenerateUniquePaymentIdAsync();
            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
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
