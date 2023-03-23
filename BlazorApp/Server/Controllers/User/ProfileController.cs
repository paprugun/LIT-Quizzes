using AutoMapper;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.Models.ResponseModels.Session;
using BlazorApp.Models.ResponseModels.User;
using BlazorApp.ResourceLibrary;
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared.Models.RequestModels.User;
using BlazorApp.Shared.Models.ResponseModel.Session;
using BlazorApp.Shared.Models.ResponseModel.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : _BaseApiController
    {
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;

        public ProfileController(IStringLocalizer<ErrorsResource> errorsLocalizer, IProfileService profileService, IMapper mapper) : base(errorsLocalizer)
        {
            _profileService = profileService;
            _mapper = mapper;
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
        public async Task<UserRoleResponseModel> Edit([FromBody]UserProfileRequestModel model)
        {
            var response = await _profileService.Edit(model);
            return response;
        }

    }
}
