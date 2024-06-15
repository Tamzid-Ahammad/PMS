using Core.DTOs;
using Core.IRepositories;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemsController(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var existingOrderItem = await _orderItemRepository.SingleOrDefaultAsync(id);

            if (existingOrderItem == null)
            {
                return NotFound();
            }

            _orderItemRepository.Remove(existingOrderItem);
            await _orderItemRepository.CommitAsync();

            return NoContent();
        }
    }

}

