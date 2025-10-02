using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1.Service.Services
{
    public class DealerService : IDealerService
    {
        private readonly ITestDriveRepository _testDriveRepo;
        private readonly ICustomerRepository _customerRepo;
        private readonly IFeedbackRepository _feedbackRepo;

        public DealerService(ITestDriveRepository testDriveRepo,
                         ICustomerRepository customerRepo,
                         IFeedbackRepository feedbackRepo)
        {
            _testDriveRepo = testDriveRepo;
            _customerRepo = customerRepo;
            _feedbackRepo = feedbackRepo;
        }

        public IEnumerable<Customer> GetAllCustomers() => _customerRepo.GetAllCustomers();

        public IEnumerable<Feedback> GetAllFeedbacks() => _feedbackRepo.GetAllFeedbacks();

        public IEnumerable<TestDrive> GetAllTestDrives() => _testDriveRepo.GetAllTestDrives();

        public void SaveCustomerProfile(Customer customer)
        {
            if(customer.CustomerId == 0) _customerRepo.AddCustomer(customer);
            else _customerRepo.UpdateCustomer(customer);
        }

        public void UpdateTestDriveStatus(int testDriveId, string status)
        {
            var testDrive = _testDriveRepo.GetTestDriveById(testDriveId);
            if (testDrive != null) { 
                testDrive.Status = status;
                _testDriveRepo.UpdateTestDrive(testDrive);
            }
        }
    }
}
