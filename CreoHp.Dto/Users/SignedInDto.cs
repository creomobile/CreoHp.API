namespace CreoHp.Dto.Users
{
    public sealed class SignedInDto : UserWithRolesDto
    {
        public string Token { get; set; }
    }
}