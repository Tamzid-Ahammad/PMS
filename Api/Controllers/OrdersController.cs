using Core.DTOs;
using Core.IRepositories;
using Core.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrdersController(IOrderRepository orderRepository,IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto orderDto)
        {
            if (orderDto.OrderItems.Count == 0)
            {
                return BadRequest("Minimum One Item Required");
            }
            using (await _orderRepository.BeginTransaction())
            {
                try
                {
                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        RecipientName = orderDto.RecipientName,
                        Email = orderDto.Email,
                        Phone = orderDto.Phone,
                        Address = orderDto.Address,
                        UserId = orderDto.UserId,
                       

                    };
                    await _orderRepository.AddAsync(order);
                    await _orderRepository.CommitAsync();
                    foreach (var item in orderDto.OrderItems)
                    {
                        await _orderItemRepository.AddAsync(new OrderItem
                        {
                            Quantity = item.Quantity,
                            VariantId = item.VariantId,
                            OrderId = order.Id,
                        });
                    }
                    await _orderItemRepository.CommitAsync();
                    await _orderRepository.CommitTransaction();
                    return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
                }
                catch (Exception)
                {
                    await _orderRepository.RollbackTransaction();
                    throw;
                }
            }
            

           

            
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var order = await _orderRepository.SingleOrDefaultAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetAsync();
            return Ok(orders);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, UpdateOrderDto orderDto)
        {
            var existingOrder = await _orderRepository.SingleOrDefaultAsync(id);

            if (existingOrder == null)
            {
                return NotFound();
            }
            using (await _orderRepository.BeginTransaction())
            {
                try
                {
                    existingOrder.RecipientName = orderDto.RecipientName;
                    existingOrder.Email = orderDto.Email;
                    existingOrder.Phone = orderDto.Phone;
                    existingOrder.Address = orderDto.Address;
                    await _orderRepository.CommitAsync();
                    foreach (var item in orderDto.OrderItems)
                    {
                        if (item.Id != null)
                        {
                            var existingOrderItem = await _orderItemRepository.SingleOrDefaultAsync(item.Id);
                            existingOrderItem.Quantity = item.Quantity;  
                        }
                        else
                        {
                            await _orderItemRepository.AddAsync(new OrderItem
                            {
                                Quantity = item.Quantity,
                                VariantId = item.VariantId,
                                OrderId = id
                            });
                        }

                    }
                 

                    await _orderItemRepository.CommitAsync();
                    await _orderRepository.CommitTransaction();

                }
                catch (Exception)
                {
                    await _orderRepository.RollbackTransaction();

                    throw;
                }
            }

            

            return NoContent();
        }

        // Delete Order
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var existingOrder = await _orderRepository.SingleOrDefaultAsync(id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            _orderRepository.Remove(existingOrder);
            await _orderRepository.CommitAsync();

            return NoContent();
        }
    }
}
