using BlazorApp.Shared.Models.ResponseModels.Quiz;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazorApp.Shared.Models.RequestModels.User;
using BlazorApp.Shared.Models.ResponseModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BlazorApp.Shared.Models.ResponseModel.User;
using Microsoft.Extensions.Localization;
using BlazorApp.ResourceLibrary;
using Blazored.Toast.Services;

namespace BlazorApp.Server.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuizController : _BaseApiController
    {
        private readonly IQuizUserService _quizUserService;

        public QuizController(IStringLocalizer<ErrorsResource> errorsLocalizer, IQuizUserService quizUserService) : base(errorsLocalizer)
        {
            _quizUserService = quizUserService;
        }

        [HttpPost("enter")]
        public async Task<JoinQuizResponseModel> EnterQuizByID([FromBody] JoinQuizRequestModel model)
        {
            var response = await _quizUserService.EnterQuizByCode(model);
            return response;
        }

        [HttpPost("pass")]
        public async Task<PassQuizResponseModel> PassQuiz([FromBody]PassQuizRequestModel model)
        {
            var response = await _quizUserService.PassQuiz(model);
            return response;
        }

    }
}
