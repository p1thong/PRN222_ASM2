using ASM1.Repository.Models;

namespace ASM1.Service.Services.Interfaces
{
    public interface IManufacturerService
    {
        Task<IEnumerable<Manufacturer>> GetAllAsync();
        Task<Manufacturer?> GetByIdAsync(int id);
        Task AddAsync(Manufacturer manufacturer);
        Task UpdateAsync(Manufacturer manufacturer);
        Task DeleteAsync(int id);
        Task<Manufacturer?> GetByNameAsync(string name);
    }
}