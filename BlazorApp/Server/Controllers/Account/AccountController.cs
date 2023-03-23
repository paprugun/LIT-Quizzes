using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared;
using BlazorApp.Shared.Models.ResponseModel.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlazorApp.Shared.Models.RequestModels.User;
using BlazorApp.Models.RequestModels;
using AutoMapper;
using BlazorApp.Shared.Models.ResponseModel.Session;
using BlazorApp.Models.ResponseModels.Session;

namespace BlazorApp.Server.Controllers.Account
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(ILogger<AccountController> logger, IUserService userService, IAccountService accountService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<RegisterResponseModel> Register([FromBody] RegisterRequestModel model)
        {
            var response = await _accountService.Register(model);
            return response;
        }
        [HttpPost("registerAdmin")]
        public async Task<RegisterResponseModel> RegisterAdmin([FromBody] RegisterRequestModel model)
        {
            var response = await _accountService.RegisterAdmin(model);
            return response;
        }


        [HttpPost("login")]
        public async Task<LoginResponseModel> Login([FromBody] LoginRequestModel model)
        {
            var response = await _accountService.Login(model);
            return response;
        }

        [HttpPost("loginAdmin")]
        public async Task<LoginResponseModel> AdminLogin([FromBody] AdminLoginRequestModel model)
        {
            var response = await _accountService.AdminLogin(model);
            return response;
        }

        [HttpGet("logout")]
        public async Task Logout()
        {
            await _accountService.Logout();
        }


    }
}
