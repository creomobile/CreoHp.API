using System;
using System.Threading.Tasks;
using CreoHp.Models.Users;

namespace CreoHp.Contracts
{
    public interface IPrincipalService
    {
        Guid? GetCurrentUserId();
        Task<AppIdentityUser> GetCurrentUser();
        Task Impersonate(AppIdentityUser user);
    }
}