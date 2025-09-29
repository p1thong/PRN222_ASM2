using ASM1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1.Service.Services.Interfaces
{
    public interface IDealerService
    {
        IEnumerable<TestDrive> GetAllTestDrives();
        void UpdateTestDriveStatus(int testDriveId, string status);
        
        IEnumerable<Customer> GetAllCustomers();
        void SaveCustomerProfile(Customer customer);

        IEnumerable<Feedback> GetAllFeedbacks();
    }
}
