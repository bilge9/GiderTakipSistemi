using Microsoft.AspNetCore.Identity;

namespace GiderTakipSistemi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string AdSoyad { get; set; } = string.Empty;
    }
}
