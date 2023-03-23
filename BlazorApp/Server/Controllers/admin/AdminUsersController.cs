using AutoMapper;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Session;
using BlazorApp.ResourceLibrary;
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared.Models.ResponseModel.Pagination;
using BlazorApp.Shared.Models.ResponseModel.Session;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admin)]
    public class AdminUsersController : _BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AdminUsersController(IStringLocalizer<ErrorsResource> errorsLocalizer, IUserService userService, IMapper mapper) : base(errorsLocalizer)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("getUsers")]
        public async Task<IEnumerable<UserRoleResponseModel>> GetUsers()
        {
            var response = await _userService.GetUsers();
            return response;

        }

        [HttpGet("getAll")]
        public async Task<IEnumerable<UserRoleResponseModel>> GetAll()
        {
            var response = await _userService.GetAll();
            return response;

        }

        [HttpGet("info/{id}")]
        public async Task<UserRoleResponseModel> GetUserInfo([FromRoute]int id)
        {
            var response = await _userService.GetUserInfo(id);
            return response;
        }

        [HttpGet("setrole/{id}-{role}")]
        public async Task SetRole([FromRoute] int id, [FromRoute] string role)
        {
            await _userService.SetRole(role,id);
        }

        [HttpDelete("deleterole/{id}-{role}")]
        public async Task DeleteRole([FromRoute] int id, [FromRoute] string role)
        {
            await _userService.DeleteRole(role, id);
        }

        [HttpDelete("hard/{userId}")]
        public async Task HardDelete([FromRoute] int userId)
        {
            await _userService.HardDeleteUser(userId);
        }

        [HttpDelete("soft/{userId}")]
        public async Task<UserResponseModel> SoftDelete([FromRoute] int userId)
        {
            var response = _userService.SoftDeleteUser(userId);
            return response;
        }
    }
}
