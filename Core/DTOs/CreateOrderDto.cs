using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CreateOrderDto
    {
        public string RecipientName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
