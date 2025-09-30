using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1.Repository.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly CarSalesDbContext _context;
        public ManagerRepository(CarSalesDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers
                .Include(c => c.Feedbacks)
                .Include(c => c.TestDrives)
                .Include(c => c.Orders)
                .Include(c => c.Quotations)
                .ToList();
        }

        public int GetTotalCustomers()
        {
            return _context.Customers.Count();
        }

        public int GetTotalFeedbacks()
        {
            return _context.Feedbacks.Count();
        }

        public int GetTotalTestDrives()
        {
            return _context.TestDrives.Count();
        }
    }
}
