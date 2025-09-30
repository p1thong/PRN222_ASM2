using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM1.Repository.Repositories
{
    public class TestDriveRepository : ITestDriveRepository
    {
        private readonly CarSalesDbContext _context;
        public TestDriveRepository(CarSalesDbContext context)
        {
            _context = context;
        }

        public void AddTestDrive(TestDrive testDrive)
        {
            _context.TestDrives.Add(testDrive);
            _context.SaveChanges();
        }

        public IEnumerable<TestDrive> GetAllTestDrives() => _context.TestDrives.ToList();


        public TestDrive GetTestDriveById(int testDriveId) => _context.TestDrives.Find(testDriveId);

        public void UpdateTestDrive(TestDrive testDrive)
        {
            _context.TestDrives.Update(testDrive);
            _context.SaveChanges();
        }
    }
}
