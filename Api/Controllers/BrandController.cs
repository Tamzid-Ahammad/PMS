using Core.DTOs;
using Core.IRepositories;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;
        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        [HttpGet("GetBrandOptions")]
        public async Task<IEnumerable<BrandInfo>> GetBrandOptions()
        {
            return await _brandRepository.GetBrandOptions();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrandById(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id);
            if (brand == null)
            {
                return NotFound("Brand not found");
            }
            return brand;
        }
        public record CreateBrandPayload(string Name);
        [Authorize]
        [HttpPost("Create")]
        public async Task Create([FromBody] CreateBrandPayload payload)
        {
            await _brandRepository.AddAsync(new Brand { Name = payload.Name });
            await _brandRepository.CommitAsync();
        }
        [Authorize]
        [HttpPost("CreateRange")]
        public async Task CreateRange([FromBody] IEnumerable<CreateBrandPayload> payload)
        {
            foreach (var item in payload)
            {
                await _brandRepository.AddAsync(new Brand { Name = item.Name });
            }
            await _brandRepository.CommitAsync();
        }
    }
}
