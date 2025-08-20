using System.Collections.Generic;

namespace GiderTakipSistemi.Models
{
    public class EditRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public IList<string> UserRoles { get; set; } = new List<string>();
        public List<string> SelectedRoles { get; set; } = new();
    }
}