﻿using System;
using System.Threading.Tasks;
using CreoHp.Api.Attributes;
using CreoHp.Common;
using CreoHp.Contracts;
using CreoHp.Dto.Pagination;
using CreoHp.Dto.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreoHp.Api.Controllers
{
    [ApiController, AuthorizeHp(UserRole.Admin), Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentException(nameof(usersService));
        }

        [HttpPost("signIn"), AllowAnonymous]
        public Task<SignedInDto> SignInAsync(SignInDto signIn) => _usersService.SignIn(signIn);

        [HttpPost("signUp"), AllowAnonymous]
        public Task<SignedInDto> SignUpAsync(SignUpDto signUp) => _usersService.SignUp(signUp, UserRole.User);

        [HttpGet("search")]
        public Task<SimplePage<UserWithRolesDto>> Search([FromQuery] UserRequestCriteria criteria) =>
            _usersService.Search(criteria);
    }
}