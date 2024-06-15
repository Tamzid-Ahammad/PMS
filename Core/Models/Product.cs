using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<Variant> Variants { get; set; }
    }
}
