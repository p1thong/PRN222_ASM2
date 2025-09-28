using ASM1.Service.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderViewModel>> GetAllAsync();
        Task<OrderViewModel?> GetByIdAsync(int id);
        Task AddAsync(OrderCreateViewModel order);
        Task UpdateAsync(OrderViewModel order);
        Task DeleteAsync(int id);
    }
}
