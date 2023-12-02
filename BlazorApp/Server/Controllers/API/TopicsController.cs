using AutoMapper;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.ResourceLibrary;
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
    public class TopicsController : _BaseApiController
    {
        private readonly ITopicsService _topicsService;

        public TopicsController(IStringLocalizer<ErrorsResource> errorsLocalizer,
                                ITopicsService topicService) : base(errorsLocalizer)
        {
            _topicsService = topicService;
        }

        [HttpGet("")]
        public async Task<IEnumerable<TopicResponseModel>> GetAllTopics()
        {
            var response = await _topicsService.GetAllTopics();
            return response;
        }

    }
}
