using CreoHp.Common;

namespace CreoHp.Dto.Users
{
    public class UserWithRolesDto : UserDto
    {
        public UserRole[] Roles { get; set; }
    }
}