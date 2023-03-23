

using AutoMapper;
using BlazorApp.Models.ResponseModels.MainPage;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.ResourceLibrary;
using BlazorApp.Services.Interfaces.PageServices;
using BlazorApp.Shared.Models.ResponseModels.Quiz;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.Main_Page
{
    [ApiController]
    [Route("[controller]")]
    public class HomePageController : _BaseApiController
    {
        private readonly IHomePageService _homePageService;
        public HomePageController(IStringLocalizer<ErrorsResource> errorsLocalizer, IHomePageService homePageService) : base(errorsLocalizer)
        {
            _homePageService = homePageService;
        }

        [HttpGet()]
        public async Task<IEnumerable<SmallQuizResponseModel>> GetFeaturesQuizzes() 
        {
            var response = await _homePageService.GetFeaturesQuizzes();
            return response;
        }

        [HttpGet("csharpinfo")]
        public async Task<CSharpCardInfo> GetCSharpInfo()
        {
            var response = await _homePageService.CSCardInfo();
            return response;
        }

        [HttpGet("jsinfo")]
        public async Task<JSCardInfo> GetJSInfo()
        {
            var response = await _homePageService.JSCardInfo();
            return response;
        }

        [HttpGet("cppinfo")]
        public async Task<CPPCardInfo> GetCPPInfo()
        {
            var response = await _homePageService.CPPCardInfo();
            return response;
        }
    }
}
