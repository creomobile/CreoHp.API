using System.Threading.Tasks;
using CreoHp.Common;
using CreoHp.Dto.Pagination;
using CreoHp.Dto.Users;

namespace CreoHp.Contracts
{
    public interface IUsersService
    {
        Task<SignedInDto> SignUp(SignUpDto signUp, params UserRole[] roles);
        Task<SignedInDto> SignIn(SignInDto signIn);
        Task<SimplePage<UserWithRolesDto>> Search(UserRequestCriteria criteria);
        Task<UserDto> GetCurrentUser();
    }
}