using Core.DTOs;
using Core.Models;

namespace Core.IRepositories
{
    public interface IProductRepository: IRepository<Product>
    {
        Task<ProductInfo> GetProductDetailsAsync(int id);
    }
}
