using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.ResourceLibrary;
using BlazorApp.Server.Controllers.API;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.API.Admin
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admin)]
    public class AdminResultsController : _BaseApiController
    {
        private readonly IResultsService _resultService;

        public AdminResultsController(IStringLocalizer<ErrorsResource> errorsLocalizer, 
                                      IResultsService resultsService
                                      ) : base(errorsLocalizer)
        {
            _resultService = resultsService;
        }

        [HttpDelete("{id}")]
        public async Task DeleteResult([FromRoute] int id)
        {
            await _resultService.DeleteResult(id);
        }

        [HttpGet("getAll/{quizId}")]
        public async Task<IEnumerable<SmallUserResultResponseModel>> GetAllResults([FromRoute] int quizId)
        {
            var response = await _resultService.GetAllResults(quizId);
            return response;
        }

        [HttpGet("getResult/{id}")]
        public async Task<UserResultResponseModel> GetResult([FromRoute] int id)
        {
            var response = await _resultService.GetResult(id);
            return response;
        }
    }
}