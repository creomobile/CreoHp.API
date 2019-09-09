using CreoHp.Common;
using Microsoft.AspNetCore.Authorization;

namespace CreoHp.Api.Attributes
{
    public sealed class AuthorizeHpAttribute : AuthorizeAttribute
    {
        public AuthorizeHpAttribute(params UserRole[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}
