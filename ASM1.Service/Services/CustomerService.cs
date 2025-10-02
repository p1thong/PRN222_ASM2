using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace ASM1.Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IUnitOfWork unitOfWork, ILogger<CustomerService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _unitOfWork.Customers.GetAllWithDealerAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return await _unitOfWork.Customers.GetByIdWithDealerAsync(id);
        }

        public async Task AddAsync(Customer customer)
        {
            customer.CustomerId = await _unitOfWork.Customers.GenerateUniqueCustomerIdAsync();
            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Thêm khách hàng thành công: {Email}", customer.Email);
        }

        public async Task UpdateAsync(Customer customer)
        {
            var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(customer.CustomerId);
            if (existingCustomer == null)
            {
                throw new InvalidOperationException($"Khách hàng với ID {customer.CustomerId} không tồn tại");
            }

            existingCustomer.FullName = customer.FullName;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Birthday = customer.Birthday;
            existingCustomer.DealerId = customer.DealerId;

            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Cập nhật khách hàng thành công: {CustomerId}", customer.CustomerId);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID khách hàng không hợp lệ");
            }

            var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                throw new InvalidOperationException($"Khách hàng với ID {id} không tồn tại");
            }

            await _unitOfWork.Customers.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Xóa khách hàng thành công: {CustomerId}", id);
        }

        public async Task<bool> IsNewCustomerAsync(int customerId)
        {
            try
            {
                return await _unitOfWork.Customers.IsNewCustomerAsync(customerId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra customer mới: {CustomerId}", customerId);
                return false;
            }
        }
    }
}
