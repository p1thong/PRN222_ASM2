using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Models;
using ASM1.Service.Services.Interfaces;
using AutoMapper;

namespace ASM1.Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CustomerViewModel>> GetAllAsync()
        {
            var customers = await _unitOfWork.Customers.GetAllAsync();
            return _mapper.Map<IEnumerable<CustomerViewModel>>(customers);
        }

        public async Task<CustomerViewModel?> GetByIdAsync(int id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            return customer == null ? null : _mapper.Map<CustomerViewModel>(customer);
        }

        public async Task AddAsync(CustomerCreateViewModel customerVm)
        {
            var customer = _mapper.Map<Customer>(customerVm);
            customer.CustomerId = await _unitOfWork.Customers.GenerateUniqueIdAsync(c => c.CustomerId);
            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();
        }


        public async Task UpdateAsync(CustomerViewModel customerVm)
        {
            var customer = _mapper.Map<Customer>(customerVm);
            await _unitOfWork.Customers.UpdateAsync(customer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Customers.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
