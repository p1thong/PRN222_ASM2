using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class DealerContractRepository : IDealerContractRepository
    {
        private readonly CarSalesDbContext _context;

        public DealerContractRepository(CarSalesDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DealerContract> GetAllDealerContracts()
        {
            return _context.DealerContracts
                .Include(dc => dc.Dealer)
                .Include(dc => dc.Manufacturer)
                .ToList();
        }

        public DealerContract? GetDealerContractById(int id)
        {
            return _context.DealerContracts
                .Include(dc => dc.Dealer)
                .Include(dc => dc.Manufacturer)
                .FirstOrDefault(dc => dc.DealerContractId == id);
        }

        public DealerContract? GetDealerContractByDealerAndManufacturer(int dealerId, int manufacturerId)
        {
            return _context.DealerContracts
                .Include(dc => dc.Dealer)
                .Include(dc => dc.Manufacturer)
                .FirstOrDefault(dc => dc.DealerId == dealerId && dc.ManufacturerId == manufacturerId);
        }

        public void AddDealerContract(DealerContract dealerContract)
        {
            _context.DealerContracts.Add(dealerContract);
            _context.SaveChanges();
        }

        public void UpdateDealerContract(DealerContract dealerContract)
        {
            _context.DealerContracts.Update(dealerContract);
            _context.SaveChanges();
        }

        public void DeleteDealerContract(int id)
        {
            var contract = _context.DealerContracts.Find(id);
            if (contract != null)
            {
                _context.DealerContracts.Remove(contract);
                _context.SaveChanges();
            }
        }

        public IEnumerable<DealerContract> GetContractsByDealer(int dealerId)
        {
            return _context.DealerContracts
                .Include(dc => dc.Dealer)
                .Include(dc => dc.Manufacturer)
                .Where(dc => dc.DealerId == dealerId)
                .ToList();
        }

        public IEnumerable<DealerContract> GetContractsByManufacturer(int manufacturerId)
        {
            return _context.DealerContracts
                .Include(dc => dc.Dealer)
                .Include(dc => dc.Manufacturer)
                .Where(dc => dc.ManufacturerId == manufacturerId)
                .ToList();
        }

        public bool IsContractActive(int dealerId, int manufacturerId)
        {
            var contract = GetDealerContractByDealerAndManufacturer(dealerId, manufacturerId);
            return contract != null && contract.SignedDate.HasValue;
        }

        public decimal GetTotalTargetSalesByManufacturer(int manufacturerId)
        {
            return _context.DealerContracts
                .Where(dc => dc.ManufacturerId == manufacturerId && dc.TargetSales.HasValue)
                .Sum(dc => dc.TargetSales.Value);
        }

        public decimal GetTotalCreditLimitByDealer(int dealerId)
        {
            return _context.DealerContracts
                .Where(dc => dc.DealerId == dealerId && dc.CreditLimit.HasValue)
                .Sum(dc => dc.CreditLimit.Value);
        }

    }
}
