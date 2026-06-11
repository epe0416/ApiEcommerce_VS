using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce_VS.Models.Dtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage ="El nombre es obligatorio.")]
        [MaxLength(50, ErrorMessage ="El nombre no puede tener más de 50 caracteres")]
        [MinLength(3, ErrorMessage = "El nombre no puede tener menos de 50 caracteres")]
        public string name { get; set; } = string.Empty;
    }
}
