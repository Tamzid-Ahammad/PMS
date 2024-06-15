using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public Guid OrderId { get; set; }
        public int VariantId { get; set; }

        public virtual Order Order { get; set; }
        public virtual Variant Variant { get; set; }
    }
}
