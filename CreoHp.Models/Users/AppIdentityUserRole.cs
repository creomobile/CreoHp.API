using System;
using Microsoft.AspNetCore.Identity;

namespace CreoHp.Models.Users
{
    public class AppIdentityUserRole : IdentityUserRole<Guid>
    {
        public virtual AppIdentityUser User { get; set; }
        public virtual AppIdentityRole Role { get; set; }
    }
}