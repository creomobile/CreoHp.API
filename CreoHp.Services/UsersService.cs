using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using CreoHp.Common;
using CreoHp.Contracts;
using CreoHp.Dto.Pagination;
using CreoHp.Dto.Users;
using CreoHp.Models.Users;
using CreoHp.Repository;
using CreoHp.Services.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CreoHp.Services
{
    public sealed class UsersService : IUsersService
    {
        readonly SignInManager<AppIdentityUser> _signInManager;
        readonly UserManager<AppIdentityUser> _userManager;
        readonly AppDbContext _dbContext;
        readonly IMapper _mapper;
        readonly IPrincipalService _principalService;
        readonly AuthConfig _authConfig;
        readonly IRolesHelper _rolesHelper;

        public UsersService(
            SignInManager<AppIdentityUser> signInManager,
            UserManager<AppIdentityUser> userManager,
            AppDbContext dbContext,
            IMapper mapper,
            IPrincipalService principalService,
            IRolesHelper rolesHelper,
            IOptionsSnapshot<AuthConfig> authConfig
        )
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _principalService = principalService ?? throw new ArgumentNullException(nameof(principalService));
            _rolesHelper = rolesHelper ?? throw new ArgumentNullException(nameof(rolesHelper));
            _authConfig = authConfig?.Value ?? throw new ArgumentNullException(nameof(authConfig));
        }

        public async Task<SignedInDto> SignUp(SignUpDto signUp, params UserRole[] roles)
        {
            if (signUp == null) throw new ArgumentException(nameof(signUp));
            if (roles.Length == 0) throw new AppException("At least one role required");

            var user = _mapper.Map<AppIdentityUser>(signUp);

            var result = await _userManager.CreateAsync(user, signUp.Password);
            if (!result.Succeeded)
                throw new AppException(result.Errors.Select(_ => _.Description).FirstOrDefault());

            await _userManager.AddToRolesAsync(user, roles.Select(_ => _.ToString()));
            await _principalService.Impersonate(user);

            return await GetToken(user);
        }

        public async Task<SignedInDto> SignIn(SignInDto signIn)
        {
            if (signIn == null) throw new ArgumentException(nameof(signIn));

            if (string.IsNullOrWhiteSpace(signIn.Email) || string.IsNullOrWhiteSpace(signIn.Password))
                ThrowUnauthorized();

            var result = await _signInManager.PasswordSignInAsync(signIn.Email, signIn.Password, false, false);

            if (!result.Succeeded) ThrowUnauthorized();

            var user = await _dbContext.Users
                .Include(_ => _.Roles)
                .SingleOrDefaultAsync(r => r.UserName == signIn.Email);

            return await GetToken(user);
        }

        public async Task<SimplePage<UserWithRolesDto>> Search(UserRequestCriteria criteria)
        {
            IQueryable<AppIdentityUser> query = _dbContext.Users;

            if (criteria.IncludeRoles)
                query = query.Include(_ => _.Roles);

            if (!string.IsNullOrWhiteSpace(criteria.Q))
            {
                query = query.Where(_ =>
                    _.FirstName.Contains(criteria.Q) || _.LastName.Contains(criteria.Q) ||
                    _.Email.Contains(criteria.Q));
            }

            if (criteria.Roles?.Any() == true)
            {
                var roles = criteria.Roles.Select(_ => _rolesHelper.GetRoleId(_));
                query = query.Where(user =>
                    user.Roles.Select(role => role.RoleId).Any(roleId => roles.Contains(roleId)));
            }

            var page = await query.GetSimplePage(criteria);
            var result = _mapper.Map<SimplePage<UserWithRolesDto>>(page);

            return result;
        }

        public Task<UserDto> GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        async Task<SignedInDto> GetToken(AppIdentityUser user)
        {
            if (user == null) throw new ArgumentException(nameof(user));
            if (user.IsBlocked) ThrowUnauthorized();

            var handler = new JwtSecurityTokenHandler();

            // ReSharper disable once PossibleNullReferenceException
            var genericIdentity = new GenericIdentity(user.UserName, JwtBearerDefaults.AuthenticationScheme);
            var claimsIdentity = new ClaimsIdentity(genericIdentity, new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.UserData, user.Id.ToString(), ClaimValueTypes.String)
            });

            if (user.Roles?.Any() != true)
                await _dbContext.Entry(user).Collection(_ => _.Roles).LoadAsync();

            // ReSharper disable once AssignNullToNotNullAttribute
            claimsIdentity.AddClaims(user.Roles.Select(_ =>
                new Claim(ClaimTypes.Role, _rolesHelper.GetRoleById(_.RoleId).ToString())));

            var securityToken = handler
                .CreateJwtSecurityToken(
                    _authConfig.Issuer,
                    _authConfig.Audience,
                    signingCredentials: new SigningCredentials(_authConfig.SecurityKey, SecurityAlgorithms.RsaSha256),
                    subject: claimsIdentity
                );

            var result = _mapper.Map<SignedInDto>(user);
            result.Token = handler.WriteToken(securityToken);

            return result;
        }

        static void ThrowUnauthorized() => throw new AppException("Unauthorized");
    }
}