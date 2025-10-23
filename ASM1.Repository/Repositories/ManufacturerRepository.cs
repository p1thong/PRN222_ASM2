using ASM1.Repository.Data;
using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Repository.Repositories
{
    public class ManufacturerRepository : GenericRepository<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(CarSalesDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Manufacturer>> GetManufacturersWithModelsAsync()
        {
            return await _context.Set<Manufacturer>()
                .Include(m => m.VehicleModels)
                .ToListAsync();
        }

        public async Task<Manufacturer?> GetByNameAsync(string name)
        {
            return await _context.Set<Manufacturer>()
                .FirstOrDefaultAsync(m => m.Name.ToLower() == name.ToLower());
        }

    }
}