using Microsoft.AspNetCore.Identity;

namespace Propelo.Models
{
    public class User:IdentityUser
    {
        public string? Initials { get; set; }

    }
}
