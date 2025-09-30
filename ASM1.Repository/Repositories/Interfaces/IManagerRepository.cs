using ASM1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IManagerRepository
    {
        int GetTotalCustomers();
        int GetTotalFeedbacks();
        int GetTotalTestDrives();
        IEnumerable<Customer> GetAllCustomers();
    }
}
