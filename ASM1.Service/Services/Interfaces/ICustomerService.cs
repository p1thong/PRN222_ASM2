using ASM1.Repository.Models;
using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ServiceResponse<IEnumerable<Customer>>> GetAllAsync();
        Task<ServiceResponse<Customer>> GetByIdAsync(int id);
        Task<ServiceResponse> AddAsync(Customer customer);
        Task<ServiceResponse> UpdateAsync(Customer customer);
        Task<ServiceResponse> DeleteAsync(int id);
        Task<bool> IsNewCustomerAsync(int customerId);
    }
}
