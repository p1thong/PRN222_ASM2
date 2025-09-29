using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Repository.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
	public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
	{
		public CustomerRepository(CarSalesDbContext context) : base(context)
		{
		}

		/// <summary>
		/// Sinh CustomerId ngẫu nhiên và kiểm tra trùng
		/// </summary>
		public async Task<int> GenerateUniqueCustomerIdAsync()
		{
			return await IdGenerator.GenerateUniqueCustomerIdAsync(_context);
		}

		/// <summary>
		/// Lấy tất cả customers với thông tin dealer
		/// </summary>
		public async Task<IEnumerable<Customer>> GetAllWithDealerAsync()
		{
			return await _context.Set<Customer>()
				.Include(c => c.Dealer)
				.ToListAsync();
		}

		/// <summary>
		/// Lấy customer theo ID với thông tin dealer
		/// </summary>
		public async Task<Customer?> GetByIdWithDealerAsync(int id)
		{
			return await _context.Set<Customer>()
				.Include(c => c.Dealer)
				.FirstOrDefaultAsync(c => c.CustomerId == id);
		}

		/// <summary>
		/// Kiểm tra xem customer có phải là khách hàng mới (chưa có order nào)
		/// </summary>
		public async Task<bool> IsNewCustomerAsync(int customerId)
		{
			var orderCount = await _context.Set<Order>()
				.Where(o => o.CustomerId == customerId)
				.CountAsync();
			return orderCount == 0;
		}
	}
}
