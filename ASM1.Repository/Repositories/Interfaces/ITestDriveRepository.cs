using ASM1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface ITestDriveRepository
    {
        IEnumerable<TestDrive> GetAllTestDrives();
        TestDrive GetTestDriveById(int testDriveId);
        void UpdateTestDrive(TestDrive testDrive);
        void AddTestDrive(TestDrive testDrive);
    }
}
