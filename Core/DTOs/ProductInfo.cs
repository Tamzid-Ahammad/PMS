using Core.Enums;
using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class ProductInfo
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public ProductType Type { get; set; }
        public string TypeInText { get; set; } 

        public  BrandInfo Brand { get; set; }
        public  IEnumerable<VariantInfo> Variants { get; set; }
    }
}
