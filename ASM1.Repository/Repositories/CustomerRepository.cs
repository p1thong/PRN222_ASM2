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
	}
}
