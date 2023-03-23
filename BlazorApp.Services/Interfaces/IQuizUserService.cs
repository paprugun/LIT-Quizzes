using BlazorApp.Models.ResponseModels;
using BlazorApp.Shared.Models.RequestModels.User;
using BlazorApp.Shared.Models.ResponseModel.User;
using BlazorApp.Shared.Models.ResponseModels.Quiz;
using BlazorApp.Shared.Models.ResponseModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces
{
    public interface IQuizUserService
    {
        Task<JoinQuizResponseModel> EnterQuizByCode(JoinQuizRequestModel model);
        Task<PassQuizResponseModel> PassQuiz(PassQuizRequestModel model);
    }
}
