using Microsoft.AspNetCore.Identity;

namespace Furn.Models.Auth
{
    public class AppRole : IdentityRole
    {
        public bool IsActivated { get; set; }
    }
}
