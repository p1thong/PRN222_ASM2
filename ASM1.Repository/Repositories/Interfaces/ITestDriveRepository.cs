using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface ITestDriveRepository : IGenericRepository<TestDrive>
    {
        IEnumerable<TestDrive> GetAllTestDrives();
        TestDrive? GetTestDriveById(int testDriveId);
        void AddTestDrive(TestDrive testDrive);
        void UpdateTestDrive(TestDrive testDrive);
        void DeleteTestDrive(int testDriveId);
    }
}