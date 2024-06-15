using Core.DTOs;
using Core.IRepositories;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context): base(context) { }
        
        private ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }

        public async Task<ProductInfo> GetProductDetailsAsync(int id)
        {
            return await ApplicationDbContext.Products.Select(p => new ProductInfo
            {
                Id = id,
                Name = p.Name,
                Brand = new BrandInfo
                {
                    Id = p.BrandId,
                    Name = p.Brand.Name

                },
                Type = p.Type,
                TypeInText = p.Type.ToString(),
                Variants = p.Variants.Select(v => new VariantInfo
                {
                    Id = v.Id,
                    Color = v.Color,
                    Price = v.Price,
                    Size = v.Size,
                    SizeInText = v.Size.ToString(),
                    ProductId = v.ProductId,
                    Specification = v.Specification
                })
            }).FirstOrDefaultAsync(it=>it.Id==id);
        }
    }
}
