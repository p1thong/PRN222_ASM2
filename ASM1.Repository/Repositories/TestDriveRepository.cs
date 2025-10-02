using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class TestDriveRepository : GenericRepository<TestDrive>, ITestDriveRepository
    {
        public TestDriveRepository(CarSalesDbContext context) : base(context)
        {
        }

        public IEnumerable<TestDrive> GetAllTestDrives()
        {
            return _context.Set<TestDrive>()
                .Include(td => td.Customer)
                .Include(td => td.Variant)
                .ToList();
        }

        public TestDrive? GetTestDriveById(int testDriveId)
        {
            return _context.Set<TestDrive>()
                .Include(td => td.Customer)
                .Include(td => td.Variant)
                .FirstOrDefault(td => td.TestDriveId == testDriveId);
        }

        public void AddTestDrive(TestDrive testDrive)
        {
            _context.Set<TestDrive>().Add(testDrive);
            _context.SaveChanges();
        }

        public void UpdateTestDrive(TestDrive testDrive)
        {
            _context.Set<TestDrive>().Update(testDrive);
            _context.SaveChanges();
        }

        public void DeleteTestDrive(int testDriveId)
        {
            var testDrive = GetTestDriveById(testDriveId);
            if (testDrive != null)
            {
                _context.Set<TestDrive>().Remove(testDrive);
                _context.SaveChanges();
            }
        }
    }
}