using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ServiceResponse<IEnumerable<CustomerViewModel>>> GetAllAsync();
        Task<ServiceResponse<CustomerViewModel>> GetByIdAsync(int id);
        Task<ServiceResponse> AddAsync(CustomerCreateViewModel customer);
        Task<ServiceResponse> UpdateAsync(CustomerViewModel customer);
        Task<ServiceResponse> DeleteAsync(int id);
    }
}
