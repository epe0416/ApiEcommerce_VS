using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEcommerce_VS.Models.Dtos
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImgUrl { get; set; }
        public string? ImgUrlLocal { get; set; }
        public IFormFile? Image { get; set; }
        public string SKU { get; set; } = string.Empty;
        public int Stock { get; set; }
        public DateTime? UpdateTime { get; set; } = null;
        public int CategoryId { get; set; }
    }
}
