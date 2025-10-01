using ASM1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1.Service.Services.Interfaces
{
    public interface ICustomerService
    {
        void ScheduleTestDrive(TestDrive testDrive);
        void SendFeedback(Feedback feedback);
    }
}
