using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using CreoHp.Common;

namespace CreoHp.Dto.Users
{
    [DebuggerDisplay("{FirstName,nq} {LastName,nq} ({Email,nq})")]
    public class UserDto
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender? Gender { get; set; }
    }
}