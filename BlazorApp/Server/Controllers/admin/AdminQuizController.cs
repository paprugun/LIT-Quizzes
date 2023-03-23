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
using BlazorApp.Models.ResponseModels.User;
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
using BlazorApp.Common.Utilities.Interfaces;

namespace BlazorApp.Server.Controllers.admin
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet("getQuizById/{id}")]
        public async Task<QuizResponseModel> GetById([FromRoute] int id)
        {
            var response = await _quizAdminService.GetQuizById(id);
            return response;
        }

        [HttpGet("getAllSmallQuizzes")]
        public async Task<IEnumerable<SmallQuizResponse>> GetAllSmall()
        {
            var response = await _quizAdminService.GetAllSmallQuizzes();
            return _mapper.Map<IEnumerable<SmallQuizResponse>>(response);
        }

        [HttpPost("")]
        public async Task<QuizResponseModel> PostNewQuiz([FromBody] QuizRequestModel model)
        {
            var response = await _quizAdminService.PostNewQuiz(model);
            return response;
        }

        [HttpPost("postQuestion")]
        public async Task<QuizQuestionResponseModel> PostNewQuestion([FromBody] QuizQuestionRequestModel model)
        {
            var response = await _quizAdminService.PostNewQuestion(model);
            return response;
        }

        [HttpPost("postAnswer")]
        public async Task<QuizAnswerResponseModel> PostNewAnswer([FromBody] QuizAnswerRequestModel model)
        {
            var response = await _quizAdminService.PostNewAnswer(model);
            return response;
        }


        [HttpDelete("deleteQuizById/{id}")]
        public async Task<string> DeleteQuiz([FromRoute] int id)
        {
            var response = await _quizAdminService.DeleteQuiz(id);
            return response;
        }

        [HttpDelete("deleteQuestionById/{id}")]
        public async Task<string> DeleteQuestion([FromRoute] int id)
        {
            var response = await _quizAdminService.DeleteQuestion(id);
            return response;
        }

        [HttpDelete("deleteAnswerById/{id}")]
        public async Task<string> DeleteAnswer([FromRoute] int id)
        {
            var response = await _quizAdminService.DeleteAnswer(id);
            return response;
        }

        [HttpPatch("changeQuizStatusById/{id}")]
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
