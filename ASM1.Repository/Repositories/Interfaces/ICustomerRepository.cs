using ASM1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int customerId);
        void UpdateCustomer(Customer customer);
        void AddCustomer(Customer customer);
    }
}
