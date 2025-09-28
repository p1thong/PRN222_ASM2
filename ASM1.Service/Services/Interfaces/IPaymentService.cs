using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentViewModel>> GetAllAsync();
        Task<PaymentViewModel?> GetByIdAsync(int id);
        Task AddAsync(PaymentCreateViewModel payment);
        Task UpdateAsync(PaymentViewModel payment);
        Task DeleteAsync(int id);
    }
}
