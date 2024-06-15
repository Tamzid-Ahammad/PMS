using Core.Models;

namespace Core.IRepositories
{
    public interface IVariantRepository : IRepository<Variant>
    {
        Task<IEnumerable<Variant>> GetByProductIdAsync(int productId);
    }
}
