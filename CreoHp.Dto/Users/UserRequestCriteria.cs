using CreoHp.Common;
using CreoHp.Dto.Pagination;

namespace CreoHp.Dto.Users
{
    public class UserRequestCriteria : PaginationCriteria
    {
        public string Q { get; set; }
        public UserRole[] Roles { get; set; }
        public bool IncludeRoles { get; set; }
    }
}