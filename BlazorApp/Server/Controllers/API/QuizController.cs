using BlazorApp.Client.Shared.Pagination;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.ResourceLibrary;
using BlazorApp.Services.Interfaces;
using BlazorApp.Services.Services;
using BlazorApp.Shared.Models.RequestModels.User;
using BlazorApp.Shared.Models.ResponseModel.User;
using BlazorApp.Shared.Models.ResponseModels.Quiz;
using BlazorApp.Shared.Models.ResponseModels.User;
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
        public async Task<PassQuizResponseModel> PassQuiz([FromBody] PassQuizRequestModel model)
        {
            var response = await _quizUserService.PassQuiz(model);
            return response;
        }

        [AllowAnonymous]
        [HttpPost("")]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequestModel<QuizTableColumn> model)
        {
            var response = _quizUserService.GetAll(model);
            return Json(new JsonPaginationResponse<List<SmallQuizResponseModel>>(response.Data, model.Offset + model.Limit, response.TotalCount));
        }

    }
}
