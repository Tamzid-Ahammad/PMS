using Core.DTOs;
using Core.Models;

namespace Core.IRepositories
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<IEnumerable<BrandInfo>> GetBrandOptions();
        Task<Brand> GetByIdAsync(int id);
    }
}
