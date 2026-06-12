using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce_VS.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Rol { get; set; }
    }
}
