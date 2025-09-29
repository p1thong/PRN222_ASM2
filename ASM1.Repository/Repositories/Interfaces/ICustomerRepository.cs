using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
	public interface ICustomerRepository : IGenericRepository<Customer>
	{
		Task<int> GenerateUniqueCustomerIdAsync();
		Task<IEnumerable<Customer>> GetAllWithDealerAsync();
		Task<Customer?> GetByIdWithDealerAsync(int id);
		Task<bool> IsNewCustomerAsync(int customerId);
	}
}
