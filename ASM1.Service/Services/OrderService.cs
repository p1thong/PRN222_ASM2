using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Models;
using ASM1.Service.Services.Interfaces;
using AutoMapper;

namespace ASM1.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderViewModel>>(orders);
        }

        public async Task<OrderViewModel?> GetByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            return order == null ? null : _mapper.Map<OrderViewModel>(order);
        }

        public async Task AddAsync(OrderCreateViewModel orderVm)
        {
            var order = _mapper.Map<Order>(orderVm);
            order.OrderId = await _unitOfWork.Orders.GenerateUniqueIdAsync(o => o.OrderId);
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(OrderViewModel orderVm)
        {
            var order = _mapper.Map<Order>(orderVm);
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
