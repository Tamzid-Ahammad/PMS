using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class OrderItemDto
    {
        public int? Id { get; set; }
        public int VariantId { get; set; }
        public double Quantity { get; set; }
    }
}
