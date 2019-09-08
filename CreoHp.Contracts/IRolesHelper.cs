using System;
using CreoHp.Common;

namespace CreoHp.Contracts
{
    public interface IRolesHelper
    {
        UserRole GetRoleById(Guid roleId);
        Guid GetRoleId(UserRole role);
    }
}