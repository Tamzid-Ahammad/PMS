using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class VariantInfo
    {
        public int Id { get; set; }
      
        public string Color { get; set; }
        
        public string Specification { get; set; }
        public Size Size { get; set; }
        public string SizeInText { get; set; } 
        public int ProductId { get; set; }
        public decimal Price { get; set; }

     
    }
}
