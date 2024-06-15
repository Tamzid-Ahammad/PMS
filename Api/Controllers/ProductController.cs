using Core.DTOs;
using Core.Enums;
using Core.IRepositories;
using Core.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IVariantRepository variantRepository;
        public ProductController(IProductRepository productRepository, IVariantRepository variantRepository)
        {
            this.productRepository = productRepository;
            this.variantRepository = variantRepository;

        }

        [HttpGet("GetAllProducts")]
        public Task<IEnumerable<Product>> GetAllProducts() =>
            productRepository.GetAsync();

        [HttpGet("GetProductDetails/{id}")]
        public async Task<ProductInfo> GetProductDetails(int id)
        {
            return await productRepository.GetProductDetailsAsync(id);
        }

        public record CreateVariantPayload(string Color, string Specification, Size Size, decimal Price);
        public record CreateProductPayload(string Name, ProductType Type, int BrandId, IEnumerable<CreateVariantPayload> Variants);
        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] CreateProductPayload request)
        {
            if (request.Variants.Count() == 0)
                return BadRequest("Atleast one variant is required");

            using (await productRepository.BeginTransaction())
            {
                try
                {
                    var product = new Product
                    {
                        BrandId = request.BrandId,
                        Name = request.Name,
                        Type = request.Type
                    };
                    await productRepository.AddAsync(product);
                    await productRepository.CommitAsync();

                    foreach (var variant in request.Variants)
                    {
                        await variantRepository.AddAsync(new Variant
                        {
                            Color = variant.Color,
                            Specification = variant.Specification,
                            Size = variant.Size,
                            Price = variant.Price,
                            ProductId = product.Id
                        });
                    }
                    await variantRepository.CommitAsync();
                    await productRepository.CommitTransaction();
                }
                catch (Exception)
                {
                    await productRepository.RollbackTransaction();
                    throw;
                }
            }
            return Ok();
        }


        public record CreateOrUpdateVariantPayload(int? Id, string Color, string Specification, Size Size, decimal Price);
        public record UpdateProductPayload(string Name, ProductType Type, int BrandId, IEnumerable<CreateOrUpdateVariantPayload> Variants);
        [Authorize]
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateProductPayload request)
        {
            if (request.Variants.Count() == 0)
                return BadRequest("Atleast one variant is required");

            var product = await productRepository.SingleOrDefaultAsync(id);
            if (product == null)
                return NotFound("Product not found");

            using (await productRepository.BeginTransaction())
            {
                try
                {
                    product.BrandId = request.BrandId;
                    product.Name = request.Name;
                    product.Type = request.Type;
                    await productRepository.CommitAsync();

                    foreach (var variant in request.Variants)
                    {
                        if (variant.Id == null)
                        {
                            await variantRepository.AddAsync(new Variant
                            {
                                Color = variant.Color,
                                Specification = variant.Specification,
                                Size = variant.Size,
                                Price = variant.Price,
                                ProductId = product.Id
                            });
                        }
                        else
                        {
                            var existingVariant = await variantRepository.SingleOrDefaultAsync(variant.Id);
                            existingVariant.Color = variant.Color;
                            existingVariant.Specification = variant.Specification;
                            existingVariant.Size = variant.Size;
                            existingVariant.Price = variant.Price;
                        }
                    }
                    await variantRepository.CommitAsync();
                    await productRepository.CommitTransaction();
                }
                catch (Exception)
                {
                    await productRepository.RollbackTransaction();
                    throw;
                }
            }
            return Ok();
        }
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await productRepository.SingleOrDefaultAsync(id);
            if (product == null)
                return NotFound("Product not found");
            productRepository.Remove(product);
            await productRepository.CommitAsync();
            return NoContent();
        }
    }
}
