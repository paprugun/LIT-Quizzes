using BlazorApp.Shared.Models.RequestModels.Quiz;
using BlazorApp.Shared.Models.ResponseModels.Quiz;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Shared.Models.ResponseModels.User;
using BlazorApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Localization;
using BlazorApp.ResourceLibrary;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Shared.Models.ResponseModel.User;
using AutoMapper;
using BlazorApp.Models.RequestModels.Quiz;
using System.Text;
using System.Text.Json;

namespace BlazorApp.Server.Controllers.API.Admin
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admin)]
    public class QuizAdminController : _BaseApiController
    {
        private readonly IQuizAdminService _quizAdminService;
        private readonly IMapper _mapper;

        public QuizAdminController(IStringLocalizer<ErrorsResource> errorsLocalizer,
                                   IQuizAdminService quizAdminService,
                                   IMapper mapper) : base(errorsLocalizer)
        {
            _quizAdminService = quizAdminService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<QuizResponseModel> GetById([FromRoute] int id)
        {
            var response = await _quizAdminService.GetQuizById(id);
            return response;
        }

        [HttpPost("")]
        public async Task<QuizResponseModel> PostNewQuiz([FromBody] QuizRequestModel model)
        {
            var response = await _quizAdminService.PostNewQuiz(model);
            return response;
        }

        [HttpPost("question")]
        public async Task<QuizQuestionResponseModel> PostNewQuestion([FromBody] QuizQuestionRequestModel model)
        {
            var response = await _quizAdminService.PostNewQuestion(model);
            return response;
        }

        [HttpPost("answer")]
        public async Task<QuizAnswerResponseModel> PostNewAnswer([FromBody] QuizAnswerRequestModel model)
        {
            var response = await _quizAdminService.PostNewAnswer(model);
            return response;
        }


        [HttpDelete("{id}")]
        public async Task<string> DeleteQuiz([FromRoute] int id)
        {
            var response = await _quizAdminService.DeleteQuiz(id);
            return response;
        }

        [HttpDelete("question/{id}")]
        public async Task<string> DeleteQuestion([FromRoute] int id)
        {
            var response = await _quizAdminService.DeleteQuestion(id);
            return response;
        }

        [HttpDelete("answer/{id}")]
        public async Task<string> DeleteAnswer([FromRoute] int id)
        {
            var response = await _quizAdminService.DeleteAnswer(id);
            return response;
        }

        [HttpPatch("{id}")]
        public async Task<QuizResponseModel> ChangeQuizStatus([FromRoute] int id)
        {
            var response = await _quizAdminService.ChangeActiveStatus(id);
            return response;
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadQuiz([FromRoute] int id)
        {
            var quiz = await _quizAdminService.GetQuizById(id);
            string json = JsonSerializer.Serialize(quiz);

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(json);

            return File(bytes, "text/plain", $"{quiz.Id}, {quiz.Name}.json");
        }


    }
}