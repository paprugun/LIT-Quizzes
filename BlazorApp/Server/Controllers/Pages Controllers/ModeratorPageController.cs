using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Models.ResponseModels.Stats;
using BlazorApp.ResourceLibrary;
using BlazorApp.Services.Interfaces;
using BlazorApp.Services.Interfaces.PageServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.Pages_Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModeratorPageController : _BaseApiController
    {
        private readonly IAdminPageService _adminService;
        public ModeratorPageController(IStringLocalizer<ErrorsResource> errorsLocalizer, IAdminPageService adminService) : base(errorsLocalizer)
        {
            _adminService = adminService;
        }

        [HttpGet("")]
        public async Task<IEnumerable<SmallQuizResponseModel>> GetTable()
        {
            var response = await _adminService.GetTable();
            return response;
        }

        [HttpGet("userstats")]
        public async Task<UsersStatsResponseModel> GetUsers()
        {
            var response = await _adminService.GetUsersStats();
            return response;
        }

        [HttpGet("quizzesstats")]
        public async Task<QuizzesStatsResponseModel> GetQuizzes()
        {
            var response = await _adminService.GetQuizzesStats();
            return response;
        }

        [HttpGet("topicstats")]
        public async Task<TopicsStatsResponseModel> GetTopics()
        {
            var response = await _adminService.GetTopicsStats();
            return response;
        }
    }
}
