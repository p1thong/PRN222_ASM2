using ASM1.Repository.Models;
using ASM1.Repository.Repositories;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ManufacturerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Manufacturer>> GetAllAsync()
        {
            return await _unitOfWork.Manufacturers.GetManufacturersWithModelsAsync();
        }

        public async Task<Manufacturer?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Manufacturers.GetByIdAsync(id);
        }

        public async Task AddAsync(Manufacturer manufacturer)
        {
            await _unitOfWork.Manufacturers.AddAsync(manufacturer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(Manufacturer manufacturer)
        {
            await _unitOfWork.Manufacturers.UpdateAsync(manufacturer);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.Manufacturers.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Manufacturer?> GetByNameAsync(string name)
        {
            var manufacturers = await _unitOfWork.Manufacturers.GetAllAsync();
            return manufacturers.FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
