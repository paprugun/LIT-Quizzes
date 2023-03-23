using AutoMapper;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Shared.Models.ResponseModels.Quiz;
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared.Models.RequestModels.Quiz;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Models.ResponseModels.User;
using BlazorApp.Shared.Models.ResponseModels.User;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Shared.Models.ResponseModel.User;
using BlazorApp.Common.Exceptions;
using BlazorApp.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Models.ResponseModels.Results;
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
            var answersToDel = _unitOfWork.Repository<QuizAnswer>().Get(x => x.QuestionId == questionToDel.Id).ToList();
            foreach (var item in answersToDel)
            {
                _unitOfWork.Repository<QuizAnswer>().Delete(item);   
            }

            _unitOfWork.Repository<QuizQuestion>().DeleteById(id);
            _unitOfWork.SaveChanges();

            var response = "question deleted";
            return response;
        }

        public async Task<string> DeleteQuiz(int id)
        {
            var quizToDel = _unitOfWork.Repository<Quiz>().Get(x => x.Id == id).FirstOrDefault();
            var questionsToDel = _unitOfWork.Repository<QuizQuestion>().Get(x => x.QuizId == quizToDel.Id).ToList();
            foreach (var item in questionsToDel)
            {
                var answersToDel = _unitOfWork.Repository<QuizAnswer>().Get(x => x.QuestionId == item.Id).ToList();
                foreach (var answer in answersToDel)
                {
                    _unitOfWork.Repository<QuizAnswer>().Delete(answer);
                }
                _unitOfWork.Repository<QuizQuestion>().Delete(item);
            }
            _unitOfWork.Repository<Quiz>().Delete(quizToDel);
            _unitOfWork.SaveChanges(); 
            var response = "deleted";
            return response;
        }
        #endregion
        #region GetAll Region

        public async Task<IEnumerable<SmallQuizResponseModel>> GetAllSmallQuizzes()
        {
            var response = new List<SmallQuizResponseModel>();
            var quizzes = _unitOfWork.Repository<Quiz>().Get(x => x.Id != 0).Include(w => w.Topic).ToList();
            foreach (var item in quizzes)
            {
                var questions = await GetAllQuestionsFromQuizById(item.Id);
                var responseQuiz = _mapper.Map<SmallQuizResponseModel>(item);
                responseQuiz.Topic = item.Topic.Name;
                responseQuiz.QuestionsCount = questions.Count();
                response.Add(responseQuiz);
            }
            return response;
        }

        public async Task<IEnumerable<QuizAnswerResponseModel>> GetAllAnswersFromQuestionById(int questionId)
        {
            var allAnswers = _unitOfWork.Repository<QuizAnswer>().Get(x => x.QuestionId == questionId);
            var response = _mapper.Map<IQueryable<QuizAnswer>, IEnumerable<QuizAnswerResponseModel>>(allAnswers);
            return response;
        }

        public async Task<IEnumerable<QuizQuestionResponseModel>> GetAllQuestionsFromQuizById(int quizId)
        {
            var allQuestions =_unitOfWork.Repository<QuizQuestion>().Get(x => x.QuizId == quizId).ToList();
            var response = _mapper.Map<List<QuizQuestion>, IEnumerable<QuizQuestionResponseModel>>(allQuestions);
            return response;
        }

        public async Task<IEnumerable<QuizResponseModel>> GetAllQuizes()
        {
            var quizzes = _unitOfWork.Repository<Quiz>().GetAll();
            var response = _mapper.Map<IList<Quiz>, IEnumerable<QuizResponseModel>>(quizzes);
            return response;
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
                Question = model.Question,
                QuizId = model.QuizId,
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

            if (topic == null)
                await _topicsService.CreateTopic(model.Topic);

            topic = _unitOfWork.Repository<Topic>().Get(x => x.Name == model.Topic).Include(w => w.Quizzes).FirstOrDefault();
            topic.Quizzes.Add(quiz);

            _unitOfWork.Repository<Quiz>().Insert(quiz);
            _unitOfWork.Repository<Topic>().Update(topic);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<QuizResponseModel>(quiz);
            return response;
        }
        #endregion

        public async Task<QuizResponseModel> GetQuizById(int id)
        {
            var quiz = _unitOfWork.Repository<Quiz>().Get(x => x.Id == id).Include(w => w.Topic).FirstOrDefault();
            if (quiz == null)
                throw new CustomException(System.Net.HttpStatusCode.NotFound, "invalid quizId", "quiz does not exist or was deleted");

            var responseModel = _mapper.Map<QuizResponseModel>(quiz);

            var questions = await GetAllQuestionsFromQuizById(id);
            var usersJoined = _unitOfWork.Repository<UsersResults>().Get(x => x.QuizId == quiz.Id)
                                                                    .Include(w => w.User)
                                                                        .ThenInclude(w => w.Profile)
                                                                    .AsQueryable();

            foreach (var item in questions)
            {
                var answers = await GetAllAnswersFromQuestionById(item.Id);
                item.Answers = answers.ToList();
            }

            responseModel.UsersJoined = _mapper.Map<IQueryable<UsersResults>, List<UserResultResponseModel>>(usersJoined);
            responseModel.Questions = questions.ToList();
            return responseModel;
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
