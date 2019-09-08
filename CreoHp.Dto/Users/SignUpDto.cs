using System.ComponentModel.DataAnnotations;

namespace CreoHp.Dto.Users
{
    public sealed class SignUpDto : UserDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}