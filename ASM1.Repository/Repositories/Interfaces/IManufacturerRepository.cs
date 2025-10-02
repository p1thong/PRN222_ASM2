using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IManufacturerRepository : IGenericRepository<Manufacturer>
    {
        Task<IEnumerable<Manufacturer>> GetManufacturersWithModelsAsync();
        Task<Manufacturer?> GetByNameAsync(string name);
    }
}