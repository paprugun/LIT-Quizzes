using BlazorApp.Shared.Models.ResponseModels.Quiz;
using BlazorApp.Shared.Models.RequestModels.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Shared.Models.ResponseModels.User;
using BlazorApp.Models.ResponseModels.User;
using BlazorApp.Shared.Models.ResponseModel.User;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.Models.RequestModels.Quiz;

namespace BlazorApp.Services.Interfaces
{
    public interface IQuizAdminService
    {

        Task<QuizResponseModel> GetQuizById(int id);
        Task<QuizResponseModel> PostNewQuiz(QuizRequestModel model);
        Task<QuizQuestionResponseModel> PostNewQuestion(QuizQuestionRequestModel model);
        Task<QuizAnswerResponseModel> PostNewAnswer(QuizAnswerRequestModel model);
        Task<IEnumerable<QuizResponseModel>> GetAllQuizes();
        Task<IEnumerable<SmallQuizResponseModel>> GetAllSmallQuizzes();
        Task<IEnumerable<QuizQuestionResponseModel>> GetAllQuestionsFromQuizById(int quizId);
        Task<IEnumerable<QuizAnswerResponseModel>> GetAllAnswersFromQuestionById(int questionId);
        Task<string> DeleteQuiz(int id);
        Task<string> DeleteQuestion(int id);
        Task<string> DeleteAnswer(int id);
        Task<QuizResponseModel> ChangeActiveStatus(int id);
        Task<QuizResponseModel> ChangeNameById(int id, string newName);
    }
}
