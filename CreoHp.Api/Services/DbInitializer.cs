using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreoHp.Common;
using CreoHp.Contracts;
using CreoHp.Models.Users;
using CreoHp.Repository;
using CreoHp.Services.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CreoHp.Api.Services
{
    sealed class DbInitializer : IRolesHelper
    {
        public static readonly DbInitializer Instance = new DbInitializer();

        Dictionary<Guid, UserRole> _roles;
        Dictionary<UserRole, Guid> _roleIds;

        DbInitializer()
        {
        }

        public async Task Initialize(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetService<AppDbContext>();

            // init roles
            _roles = dbContext.Roles.ToDictionary(_ => _.Id,
                _ => Enum.TryParse(_.Name, out UserRole role) ? role : throw new AppException("Invalid roles in db"));
            var missedRoles = Enum.GetValues(typeof(UserRole)).OfType<UserRole>().Except(_roles.Values).ToArray();
            if (missedRoles.Any())
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<AppIdentityRole>>();
                foreach (var role in missedRoles)
                {
                    var appRole = new AppIdentityRole(role.ToString());
                    await roleManager.CreateAsync(appRole);
                    _roles.Add(appRole.Id, role);
                }
            }

            _roleIds = _roles.ToDictionary(_ => _.Value, _ => _.Key);

            // init first admin user
            if (!await dbContext.Users.AnyAsync())
            {
                var usersService = serviceProvider.GetRequiredService<IUsersService>();
                var authConfig = serviceProvider.GetRequiredService<IOptionsSnapshot<AuthConfig>>().Value;
                await usersService.SignUp(authConfig.InitialAdmin, UserRole.Admin);
            }
        }

        public UserRole GetRoleById(Guid roleId) => _roles[roleId];
        public Guid GetRoleId(UserRole role) => _roleIds[role];
    }
}