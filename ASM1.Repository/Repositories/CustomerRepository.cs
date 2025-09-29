using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1.Repository.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private CarSalesDbContext _context;
        public CustomerRepository(CarSalesDbContext context)
        {
            _context = context;
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public IEnumerable<Customer> GetAllCustomers() => _context.Customers.ToList();

        public Customer GetCustomerById(int customerId) => _context.Customers.Find(customerId);

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }
    }
}
