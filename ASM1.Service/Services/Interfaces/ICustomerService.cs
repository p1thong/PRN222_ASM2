using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(int id);
        Task<bool> IsNewCustomerAsync(int customerId);
    }
}
