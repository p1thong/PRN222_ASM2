using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
	public interface ICustomerRepository : IGenericRepository<Customer>
	{
		Task<int> GenerateUniqueCustomerIdAsync();
	}
}
