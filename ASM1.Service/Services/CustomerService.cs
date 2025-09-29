using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Models;
using ASM1.Service.Services.Interfaces;
using ASM1.Service.Exceptions;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ASM1.Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CustomerService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceResponse<IEnumerable<CustomerViewModel>>> GetAllAsync()
        {
            try
            {
                var customers = await _unitOfWork.Customers.GetAllAsync();
                var customerViewModels = _mapper.Map<IEnumerable<CustomerViewModel>>(customers);
                return ServiceResponse<IEnumerable<CustomerViewModel>>.SuccessResponse(customerViewModels, "Lấy danh sách khách hàng thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách khách hàng");
                return ServiceResponse<IEnumerable<CustomerViewModel>>.ErrorResponse("Không thể lấy danh sách khách hàng", ex.Message);
            }
        }

        public async Task<ServiceResponse<CustomerViewModel>> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResponse<CustomerViewModel>.ErrorResponse("ID khách hàng không hợp lệ", "ID phải lớn hơn 0");
                }

                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                {
                    return ServiceResponse<CustomerViewModel>.ErrorResponse("Không tìm thấy khách hàng", $"Khách hàng với ID {id} không tồn tại");
                }

                var customerViewModel = _mapper.Map<CustomerViewModel>(customer);
                return ServiceResponse<CustomerViewModel>.SuccessResponse(customerViewModel, "Lấy thông tin khách hàng thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin khách hàng với ID {Id}", id);
                return ServiceResponse<CustomerViewModel>.ErrorResponse("Không thể lấy thông tin khách hàng", ex.Message);
            }
        }

        public async Task<ServiceResponse> AddAsync(CustomerCreateViewModel customerVm)
        {
            try
            {
                Console.WriteLine("=== CUSTOMER SERVICE ADD DEBUG ===");
                Console.WriteLine($"Input: DealerId={customerVm.DealerId}, FullName={customerVm.FullName}, Email={customerVm.Email}, Phone={customerVm.Phone}, Birthday={customerVm.Birthday}");
                
                // Validation
                var validationErrors = await ValidateCustomerAsync(customerVm);
                Console.WriteLine($"Validation errors count: {validationErrors.Count}");
                if (validationErrors.Any())
                {
                    Console.WriteLine("Validation errors:");
                    foreach (var error in validationErrors)
                    {
                        Console.WriteLine($"  - {error}");
                    }
                    return ServiceResponse.ErrorResponse("Dữ liệu không hợp lệ", validationErrors);
                }

                var customer = _mapper.Map<Customer>(customerVm);
                Console.WriteLine($"Mapped customer: CustomerId={customer.CustomerId}, DealerId={customer.DealerId}, FullName={customer.FullName}, Email={customer.Email}");
                
                customer.CustomerId = await _unitOfWork.Customers.GenerateUniqueCustomerIdAsync();
                Console.WriteLine($"Generated CustomerId: {customer.CustomerId}");
                
                await _unitOfWork.Customers.AddAsync(customer);
                Console.WriteLine("Customer added to context");
                
                await _unitOfWork.SaveChangesAsync();
                Console.WriteLine("Changes saved successfully");
                
                _logger.LogInformation("Thêm khách hàng thành công: {Email}", customerVm.Email);
                return ServiceResponse.SuccessResponse("Thêm khách hàng thành công");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in CustomerService.AddAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                _logger.LogError(ex, "Lỗi khi thêm khách hàng: {Email}", customerVm.Email);
                return ServiceResponse.ErrorResponse("Không thể thêm khách hàng", ex.Message);
            }
        }


        public async Task<ServiceResponse> UpdateAsync(CustomerViewModel customerVm)
        {
            try
            {
                var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(customerVm.CustomerId);
                if (existingCustomer == null)
                {
                    return ServiceResponse.ErrorResponse("Không tìm thấy khách hàng", $"Khách hàng với ID {customerVm.CustomerId} không tồn tại");
                }

                var customer = _mapper.Map<Customer>(customerVm);
                await _unitOfWork.Customers.UpdateAsync(customer);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("Cập nhật khách hàng thành công: {CustomerId}", customerVm.CustomerId);
                return ServiceResponse.SuccessResponse("Cập nhật khách hàng thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật khách hàng: {CustomerId}", customerVm.CustomerId);
                return ServiceResponse.ErrorResponse("Không thể cập nhật khách hàng", ex.Message);
            }
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return ServiceResponse.ErrorResponse("ID khách hàng không hợp lệ", "ID phải lớn hơn 0");
                }

                var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (existingCustomer == null)
                {
                    return ServiceResponse.ErrorResponse("Không tìm thấy khách hàng", $"Khách hàng với ID {id} không tồn tại");
                }

                await _unitOfWork.Customers.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("Xóa khách hàng thành công: {CustomerId}", id);
                return ServiceResponse.SuccessResponse("Xóa khách hàng thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa khách hàng: {CustomerId}", id);
                return ServiceResponse.ErrorResponse("Không thể xóa khách hàng", ex.Message);
            }
        }

        private async Task<List<string>> ValidateCustomerAsync(CustomerCreateViewModel customerVm)
        {
            var errors = new List<string>();

            // Kiểm tra email đã tồn tại
            var existingCustomers = await _unitOfWork.Customers.FindAsync(c => c.Email == customerVm.Email);
            if (existingCustomers.Any())
            {
                errors.Add("Email đã được sử dụng");
            }

            // Kiểm tra DealerId hợp lệ
            if (customerVm.DealerId <= 0)
            {
                errors.Add("Dealer ID không hợp lệ");
            }

            return errors;
        }
    }
}
