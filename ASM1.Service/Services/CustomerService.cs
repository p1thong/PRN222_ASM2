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
    public class CustomerService : ICustomerService
    {
        private readonly ITestDriveRepository _testDriveRepo;
        private readonly IFeedbackRepository _feedbackRepo;

        public CustomerService(ITestDriveRepository testDriveRepo,
                               IFeedbackRepository feedbackRepo)
        {
            _testDriveRepo = testDriveRepo;
            _feedbackRepo = feedbackRepo;
        }

        public void ScheduleTestDrive(TestDrive testDrive)
        {
            testDrive.Status = "Pending";
            _testDriveRepo.AddTestDrive(testDrive);
        }

        public void SendFeedback(Feedback feedback) => _feedbackRepo.AddFeedback(feedback);
    }
}
