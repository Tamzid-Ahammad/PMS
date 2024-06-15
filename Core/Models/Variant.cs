using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Variant
    {
        public int Id { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string Specification { get; set; }
        public Size Size { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
