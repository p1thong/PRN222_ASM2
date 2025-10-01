using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly CarSalesDbContext _context;

        public PromotionRepository(CarSalesDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Promotion> GetAllPromotions()
        {
            return _context.Promotions
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Dealer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                            .ThenInclude(vm => vm.Manufacturer)
                .ToList();
        }

        public Promotion? GetPromotionById(int id)
        {
            return _context.Promotions
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Dealer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                            .ThenInclude(vm => vm.Manufacturer)
                .FirstOrDefault(p => p.PromotionId == id);
        }

        public void AddPromotion(Promotion promotion)
        {
            _context.Promotions.Add(promotion);
            _context.SaveChanges();
        }

        public void UpdatePromotion(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            _context.SaveChanges();
        }

        public void DeletePromotion(int id)
        {
            var promotion = _context.Promotions.Find(id);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Promotion> GetPromotionsByOrder(int orderId)
        {
            return _context.Promotions
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Dealer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                            .ThenInclude(vm => vm.Manufacturer)
                .Where(p => p.OrderId == orderId)
                .ToList();
        }

        public IEnumerable<Promotion> GetActivePromotions()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return _context.Promotions
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Dealer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                            .ThenInclude(vm => vm.Manufacturer)
                .Where(p => p.ValidUntil == null || p.ValidUntil >= today)
                .ToList();
        }

        public IEnumerable<Promotion> GetExpiredPromotions()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return _context.Promotions
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Dealer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                            .ThenInclude(vm => vm.Manufacturer)
                .Where(p => p.ValidUntil != null && p.ValidUntil < today)
                .ToList();
        }

        public IEnumerable<Promotion> GetPromotionsByCode(string promotionCode)
        {
            return _context.Promotions
                .Include(p => p.Order)
                    .ThenInclude(o => o.Customer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Dealer)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Variant)
                        .ThenInclude(v => v.VehicleModel)
                            .ThenInclude(vm => vm.Manufacturer)
                .Where(p => p.PromotionCode == promotionCode)
                .ToList();
        }

        public bool IsPromotionCodeValid(string promotionCode)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return _context.Promotions
                .Any(p => p.PromotionCode == promotionCode && 
                         (p.ValidUntil == null || p.ValidUntil >= today));
        }

        public bool IsPromotionActive(int promotionId)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return _context.Promotions
                .Any(p => p.PromotionId == promotionId && 
                         (p.ValidUntil == null || p.ValidUntil >= today));
        }

        public decimal GetTotalDiscountByOrder(int orderId)
        {
            return _context.Promotions
                .Where(p => p.OrderId == orderId)
                .Sum(p => p.DiscountAmount);
        }
    }
}
