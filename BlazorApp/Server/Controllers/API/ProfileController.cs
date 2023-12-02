using AutoMapper;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.Models.ResponseModels.Session;
using BlazorApp.ResourceLibrary;
using BlazorApp.Server.Helpers.Attributes;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.API
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.User)]
    [Validate]
    public class ProfileController : _BaseApiController
    {
        private readonly IProfileService _profileService;

        public ProfileController(IStringLocalizer<ErrorsResource> errorsLocalizer, IProfileService profileService) : base(errorsLocalizer)
        {
            _profileService = profileService;
        }

        [HttpGet("getAllResults")]
        public async Task<IEnumerable<SmallUserResultResponseModel>> GetAll()
        {
            var response = await _profileService.GetAllResults();
            return response;
        }

        [HttpDelete("delete")]
        public async Task Delete()
        {
            await _profileService.DeleteAccount();
        }

        [HttpDelete("deleteResult/{resultId}")]
        public async Task DeleteResult([FromRoute] int resultId)
        {
            await _profileService.DeleteResult(resultId);
        }

        [HttpGet("getResult/{resultId}")]
        public async Task<UserResultResponseModel> GetResult([FromRoute] int resultId)
        {
            var response = await _profileService.GetResult(resultId);
            return response;
        }

        [HttpPost("edit")]
        public async Task<UserRoleResponseModel> Edit([FromBody] UserProfileRequestModel model)
        {
            var response = await _profileService.Edit(model);
            return response;
        }

    }
}
