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
    public class ManagerService : IManagerService
    {
        private readonly IManagerRepository _managerRepo;
        public ManagerService(IManagerRepository managerRepo)
        {
            _managerRepo = managerRepo;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _managerRepo.GetAllCustomers();
        }

        public int GetTotalCustomers()
        {
            return _managerRepo.GetTotalCustomers();
        }

        public int GetTotalFeedbacks()
        {
            return _managerRepo.GetTotalFeedbacks();
        }

        public int GetTotalTestDrives()
        {
            return _managerRepo.GetTotalTestDrives();
        }
    }
}
