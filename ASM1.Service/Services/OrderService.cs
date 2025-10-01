using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _unitOfWork.Orders.GetAllWithDetailsAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);
        }

        public async Task AddAsync(Order order)
        {
            order.OrderId = await _unitOfWork.Orders.GenerateUniqueOrderIdAsync();
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Orders.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
