using System;
using System.Collections.Generic;
using CreoHp.Common;
using Microsoft.AspNetCore.Identity;

namespace CreoHp.Models.Users
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Gender? Gender { get; set; }

        public virtual ICollection<AppIdentityUserRole> Roles { get; set; }
    }
}