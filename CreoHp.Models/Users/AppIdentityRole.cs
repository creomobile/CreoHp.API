using System;
using System.Collections.Generic;
using CreoHp.Common;
using Microsoft.AspNetCore.Identity;

namespace CreoHp.Models.Users
{
    public class AppIdentityRole : IdentityRole<Guid>
    {
        public AppIdentityRole()
        {
        }

        public AppIdentityRole(string roleName) : base(roleName)
        {
        }

        public UserRole Role => Enum.TryParse(Name, out UserRole res)
            ? res
            : throw new AppException("Invalid user role in db");

        public virtual ICollection<AppIdentityUserRole> Users { get; set; }
    }
}