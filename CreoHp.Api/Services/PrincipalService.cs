using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CreoHp.Contracts;
using CreoHp.Models.Users;
using CreoHp.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CreoHp.Api.Services
{
    public sealed class PrincipalService : IPrincipalService
    {
        const string CurrentUserKey = "CURRENT_USER";
        const string CurrentUserIdKey = "CURRENT_USER_ID";

        readonly AppDbContext _dbContext;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly SignInManager<AppIdentityUser> _signInManager;

        public PrincipalService(
            AppDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            SignInManager<AppIdentityUser> signInManager
        )
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        internal static Guid? GetCurrentUserId(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor?.HttpContext;

            object obj = null;
            if (httpContext?.Items.TryGetValue(CurrentUserIdKey, out obj) == true)
                return (Guid) obj;

            var claim = httpContext?.User?.FindFirst(ClaimTypes.UserData);
            if (!Guid.TryParse(claim?.Value, out var id)) return null;

            // ReSharper disable once PossibleNullReferenceException
            httpContext.Items[CurrentUserIdKey] = id;
            return id;
        }

        public Guid? GetCurrentUserId() => GetCurrentUserId(_httpContextAccessor);

        public async Task<AppIdentityUser> GetCurrentUser()
        {
            var httpContext = _httpContextAccessor?.HttpContext;

            object obj = null;
            if (httpContext?.Items.TryGetValue(CurrentUserKey, out obj) == true)
                return (AppIdentityUser) obj;

            var id = GetCurrentUserId();
            if (!id.HasValue) return null;

            var result = await _dbContext.Users
                .FirstOrDefaultAsync(p => p.Id == id);

            // ReSharper disable once PossibleNullReferenceException
            httpContext.Items[CurrentUserKey] = result;
            return result;
        }

        public async Task Impersonate(AppIdentityUser user)
        {
            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext == null) return;

            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var currentUserId = GetCurrentUserId();
            var identity = principal.Identities.First();

            if (currentUserId.HasValue)
                await _signInManager.SignOutAsync();

            var id = user.Id;
            identity.AddClaim(new Claim(ClaimTypes.UserData, id.ToString()));
            await httpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
            httpContext.User = principal;
            httpContext.Items[CurrentUserIdKey] = id;
            httpContext.Items[CurrentUserKey] = user;
        }
    }
}