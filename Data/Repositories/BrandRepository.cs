using Core.DTOs;
using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext context) : base(context) { }

        private ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }

        public async Task<IEnumerable<BrandInfo>> GetBrandOptions()
        {
            return await ApplicationDbContext.Brands.Select(b => new BrandInfo
            {
                Id = b.Id,
                Name = b.Name,
            }).ToListAsync();
        }
        public async Task<Brand> GetByIdAsync(int id)
        {
            return await ApplicationDbContext.Brands.FindAsync(id);
        }
    }
}
