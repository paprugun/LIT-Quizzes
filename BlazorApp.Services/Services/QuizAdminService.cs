using AutoMapper;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Models.RequestModels.Quiz;

namespace BlazorApp.Services.Services
{
    public class QuizAdminService : IQuizAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITopicsService _topicsService;

        public QuizAdminService(IUnitOfWork unitOfWork, IMapper mapper, ITopicsService topicsService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _topicsService = topicsService;
        }


        #region Del Region
        public async Task<string> DeleteAnswer(int id)
        {
            var response = "answer deleted";
            _unitOfWork.Repository<QuizAnswer>().DeleteById(id);
            _unitOfWork.SaveChanges();
            return response;
        }

        public async Task<string> DeleteQuestion(int id)
        {
            var questionToDel = _unitOfWork.Repository<QuizQuestion>().Get(x => x.Id == id).FirstOrDefault();

            _unitOfWork.Repository<QuizQuestion>().DeleteById(id);
            _unitOfWork.SaveChanges();

            var response = "question deleted";
            return response;
        }

        public async Task<string> DeleteQuiz(int id)
        {
            var quizToDel = _unitOfWork.Repository<Quiz>().Get(x => x.Id == id).FirstOrDefault();

            _unitOfWork.Repository<Quiz>().Delete(quizToDel);
            _unitOfWork.SaveChanges();
            
            return "deleted";
        }
        #endregion
        #region PostMethod Region
        public async Task<QuizAnswerResponseModel> PostNewAnswer(QuizAnswerRequestModel model)
        {
            var answer = new QuizAnswer()
            {
                Text = model.Text,
                QuestionId = model.QuestionId,
                IsCorrect = model.IsCorrect,
                CreatedAt = System.DateTime.UtcNow,
            };

            _unitOfWork.Repository<QuizAnswer>().Insert(answer);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<QuizAnswerResponseModel>(answer);
            return response;
        }

        public async Task<QuizQuestionResponseModel> PostNewQuestion(QuizQuestionRequestModel model)
        {
            var question = new QuizQuestion()
            {
                QuizId = model.QuizId,
                Question = model.Question,
                CreatedAt = System.DateTime.UtcNow,
            };

            _unitOfWork.Repository<QuizQuestion>().Insert(question);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<QuizQuestionResponseModel>(question);
            return response;
        }

        public async Task<QuizResponseModel> PostNewQuiz(QuizRequestModel model)
        {
            var quiz = new Quiz()
            {
                Name = model.Name,
                Author = model.Author,
                CreatedAt = System.DateTime.UtcNow,
                TimeToPass = model.TimeToPass,
                IsActive = true,
            };
            var topic = _unitOfWork.Repository<Topic>().Get(x => x.Name == model.Topic).Include(w => w.Quizzes).FirstOrDefault();

            if (topic == null && model.Topic != null)
                await _topicsService.CreateTopic(model.Topic);

            quiz.Topic = topic;
            _unitOfWork.Repository<Quiz>().Insert(quiz);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<QuizResponseModel>(quiz);
            return response;
        }
        #endregion

        public async Task<QuizResponseModel> GetQuizById(int id)
        {
            var quiz = _unitOfWork.Repository<Quiz>().Get(x => x.Id == id).Include(w => w.Topic)
                                                                          .Include(w => w.Questions)
                                                                            .ThenInclude(w => w.Answers)
                                                                          .FirstOrDefault();
            if (quiz == null)
                throw new CustomException(System.Net.HttpStatusCode.NotFound, "invalid quizId", "quiz does not exist or was deleted");

            var response = _mapper.Map<QuizResponseModel>(quiz);
            return response;
        }

        public async Task<QuizResponseModel> ChangeActiveStatus(int id)
        {
            var quizToChange = _unitOfWork.Repository<Quiz>().Get(x => x.Id == id).FirstOrDefault();
            switch (quizToChange.IsActive)
            {
                case true:
                    quizToChange.IsActive = false;
                    break;
                case false:
                    quizToChange.IsActive = true;
                    break;
            }

            _unitOfWork.Repository<Quiz>().Update(quizToChange);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<QuizResponseModel>(quizToChange);
            return response;
        }

        public async Task<QuizResponseModel> ChangeNameById(int id, string newName)
        {
            var quizToChange = _unitOfWork.Repository<Quiz>().Get(x => x.Id == id).FirstOrDefault();
            quizToChange.Name = newName;

            _unitOfWork.Repository<Quiz>().Update(quizToChange);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<QuizResponseModel>(quizToChange);
            return response;
        }

        
    }
}
