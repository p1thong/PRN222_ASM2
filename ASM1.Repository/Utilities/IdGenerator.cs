using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Utilities
{
    public static class IdGenerator
    {
        /// <summary>
        /// Sinh Customer ID ngẫu nhiên
        /// </summary>
        public static async Task<int> GenerateUniqueCustomerIdAsync(DbContext context)
        {
            var rand = new Random();
            int id;
            bool exists;
            do
            {
                id = rand.Next(1_000_000, 9_999_999);
                exists = await context.Set<ASM1.Repository.Models.Customer>().AnyAsync(c => c.CustomerId == id);
            } while (exists);
            return id;
        }

        /// <summary>
        /// Sinh Order ID ngẫu nhiên
        /// </summary>
        public static async Task<int> GenerateUniqueOrderIdAsync(DbContext context)
        {
            var rand = new Random();
            int id;
            bool exists;
            do
            {
                id = rand.Next(1_000_000, 9_999_999);
                exists = await context.Set<ASM1.Repository.Models.Order>().AnyAsync(o => o.OrderId == id);
            } while (exists);
            return id;
        }

        /// <summary>
        /// Sinh Payment ID ngẫu nhiên
        /// </summary>
        public static async Task<int> GenerateUniquePaymentIdAsync(DbContext context)
        {
            var rand = new Random();
            int id;
            bool exists;
            do
            {
                id = rand.Next(1_000_000, 9_999_999);
                exists = await context.Set<ASM1.Repository.Models.Payment>().AnyAsync(p => p.PaymentId == id);
            } while (exists);
            return id;
        }

        /// <summary>
        /// Sinh Quotation ID ngẫu nhiên
        /// </summary>
        public static async Task<int> GenerateUniqueQuotationIdAsync(DbContext context)
        {
            var rand = new Random();
            int id;
            bool exists;
            do
            {
                id = rand.Next(1_000_000, 9_999_999);
                exists = await context.Set<ASM1.Repository.Models.Quotation>().AnyAsync(q => q.QuotationId == id);
            } while (exists);
            return id;
        }

        /// <summary>
        /// Sinh SalesContract ID ngẫu nhiên
        /// </summary>
        public static async Task<int> GenerateUniqueSalesContractIdAsync(DbContext context)
        {
            var rand = new Random();
            int id;
            bool exists;
            do
            {
                id = rand.Next(1_000_000, 9_999_999);
                exists = await context.Set<ASM1.Repository.Models.SalesContract>().AnyAsync(s => s.SaleContractId == id);
            } while (exists);
            return id;
        }
    }
}