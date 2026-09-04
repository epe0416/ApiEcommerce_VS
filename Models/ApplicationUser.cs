using Microsoft.AspNetCore.Identity;

namespace ApiEcommerce_VS.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? name { get; set; }

    }
}
