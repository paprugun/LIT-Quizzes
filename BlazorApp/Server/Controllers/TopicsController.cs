using AutoMapper;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.ResourceLibrary;
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared.Models.ResponseModel.Quiz;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admin)]
    public class TopicsController : _BaseApiController
    {
        private readonly ITopicsService _topicsService;
        private readonly IMapper _mapper;

        public TopicsController(IStringLocalizer<ErrorsResource> errorsLocalizer,
                                ITopicsService topicService,
                                IMapper mapper) : base(errorsLocalizer)
        {
            _topicsService = topicService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IEnumerable<TopicResponseModel>> GetAllTopics()
        {
            var response = await _topicsService.GetAllTopics();
            return response;
        }

    }
}
