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

        public async Task<ServiceResponse<IEnumerable<Customer>>> GetAllAsync()
        {
            try
            {
                var customers = await _unitOfWork.Customers.GetAllAsync();
                return ServiceResponse<IEnumerable<Customer>>.SuccessResponse(customers);
            }
            catch (Exception ex)
            {
                return ServiceResponse<IEnumerable<Customer>>.ErrorResponse("Error getting customers", ex.Message);
            }
        }

        public async Task<ServiceResponse<Customer>> GetByIdAsync(int id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                {
                    return ServiceResponse<Customer>.ErrorResponse("Customer not found");
                }
                return ServiceResponse<Customer>.SuccessResponse(customer);
            }
            catch (Exception ex)
            {
                return ServiceResponse<Customer>.ErrorResponse("Error getting customer", ex.Message);
            }
        }

        public async Task<ServiceResponse> AddAsync(Customer customer)
        {
            try
            {
                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse.SuccessResponse("Customer added successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.ErrorResponse("Error adding customer", ex.Message);
            }
        }

        public async Task<ServiceResponse> UpdateAsync(Customer customer)
        {
            try
            {
                var existingCustomer = await _unitOfWork.Customers.GetByIdAsync(customer.CustomerId);
                if (existingCustomer == null)
                {
                    return ServiceResponse.ErrorResponse("Customer not found");
                }

                await _unitOfWork.Customers.UpdateAsync(customer);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse.SuccessResponse("Customer updated successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.ErrorResponse("Error updating customer", ex.Message);
            }
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                {
                    return ServiceResponse.ErrorResponse("Customer not found");
                }

                await _unitOfWork.Customers.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
                return ServiceResponse.SuccessResponse("Customer deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResponse.ErrorResponse("Error deleting customer", ex.Message);
            }
        }

        public async Task<bool> IsNewCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(customerId);
                if (customer == null) return false;
                
                // Logic to determine if customer is new - simplified version
                // You can implement this based on your business logic
                return true; // Placeholder - implement your logic here
            }
            catch
            {
                return false;
            }
        }
    }
}