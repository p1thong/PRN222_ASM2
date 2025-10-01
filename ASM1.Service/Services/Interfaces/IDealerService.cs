using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IDealerService
    {
        IEnumerable<Customer> GetAllCustomers();
        IEnumerable<Feedback> GetAllFeedbacks();
        IEnumerable<TestDrive> GetAllTestDrives();
        void SaveCustomerProfile(Customer customer);
        void UpdateTestDriveStatus(int testDriveId, string status);
    }
}