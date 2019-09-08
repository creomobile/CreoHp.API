using System.ComponentModel.DataAnnotations;

namespace CreoHp.Dto.Users
{
    public sealed class SignInDto
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}