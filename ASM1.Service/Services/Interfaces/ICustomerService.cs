using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<CustomerViewModel>> GetAllAsync();
        Task<CustomerViewModel?> GetByIdAsync(int id);
    Task AddAsync(CustomerCreateViewModel customer);
        Task UpdateAsync(CustomerViewModel customer);
        Task DeleteAsync(int id);
    }
}
