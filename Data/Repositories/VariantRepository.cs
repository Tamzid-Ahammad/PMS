using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class VariantRepository : Repository<Variant>, IVariantRepository
    {
        public VariantRepository(ApplicationDbContext context) : base(context) { }

        private ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
        public async Task<IEnumerable<Variant>> GetByProductIdAsync(int productId)
        {
            return await ApplicationDbContext.Variants.Where(v => v.ProductId == productId).ToListAsync();
        }
    }
}
