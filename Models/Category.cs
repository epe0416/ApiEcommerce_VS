using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce_VS.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string name { get; set; } = string.Empty;
        [Required]
        public DateTime CreationDate {  get; set; }
    }
}
