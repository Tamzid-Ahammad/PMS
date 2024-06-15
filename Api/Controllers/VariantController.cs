using Core.IRepositories;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VariantController : ControllerBase
    {
        private readonly IVariantRepository _variantRepository;
        public VariantController(IVariantRepository variantRepository)
        {
            _variantRepository = variantRepository;
        }
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var variant = await _variantRepository.SingleOrDefaultAsync(id);
            if (variant == null)
                return NotFound("Variant not found");
            _variantRepository.Remove(variant);
            await _variantRepository.CommitAsync();
            return NoContent();
        }
    }
}
