using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.ResourceLibrary;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admin)]
    public class AdminTopicsController : _BaseApiController
    {
        private readonly ITopicsService _topicsService;

        public AdminTopicsController(IStringLocalizer<ErrorsResource> errorsLocalizer, ITopicsService topicsService) : base(errorsLocalizer)
        {
            _topicsService = topicsService;
        }

        [HttpGet("{name}")]
        public async Task<TopicResponseModel> CreateNewTopic([FromRoute] string name)
        {
            var response = await _topicsService.CreateTopic(name);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task DeleteTopic([FromRoute] int id)
        {
            await _topicsService.DeleteTopic(id);
        }
    }
}
