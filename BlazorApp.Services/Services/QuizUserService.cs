using AutoMapper;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Shared.Models.RequestModels.User;
using BlazorApp.Shared.Models.ResponseModels.Quiz;
using BlazorApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Common.Exceptions;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Shared.Models.ResponseModels.User;
using BlazorApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using BlazorApp.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Shared.Models.ResponseModel.User;


namespace BlazorApp.Services.Services
{
    public class QuizUserService : IQuizUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = null;

        private bool _isUserSuperAdmin = false;
        private bool _isUserAdmin = false;
        private int? _userId = null;

        public QuizUserService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

            var context = httpContextAccessor.HttpContext;

            if (context?.User != null)
            {
                _isUserSuperAdmin = context.User.IsInRole(Role.SuperAdmin);
                _isUserAdmin = context.User.IsInRole(Role.Admin);

                try
                {
                    _userId = context.User.GetUserId();
                }
                catch
                {
                    _userId = null;
                }
            }
        }

        public async Task<JoinQuizResponseModel> EnterQuizByCode(JoinQuizRequestModel model)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == _userId)
                .Include(w => w.QuizzesResults)
                    .ThenInclude(w => w.Quiz)
                    .FirstOrDefault();

            var quiz = _unitOfWork.Repository<Quiz>().Get(x => x.Id == model.QuizId).FirstOrDefault();
            var questions = _unitOfWork.Repository<QuizQuestion>().Get(x => x.QuizId == quiz.Id);
            
            if (user == null)
                throw new CustomException(System.Net.HttpStatusCode.Unauthorized, "Unauthorized", "Unauthorized user cannot access quiz");
            if (quiz == null)
                throw new CustomException(System.Net.HttpStatusCode.Forbidden, "invalid quizId", "quiz code is invalid or quiz does not exist");
            if (quiz.IsActive == false)
                throw new CustomException(System.Net.HttpStatusCode.Locked, "quiz closed", "quiz was closed");

            if (user.QuizzesResults.FirstOrDefault(x => x.QuizId == quiz.Id) == null)
            {
                user.QuizzesResults.Add(new UsersResults() {QuizId = quiz.Id, UserId = user.Id, JoinedAt = DateTime.Now, ResultMark = 0 });
                _unitOfWork.Repository<ApplicationUser>().Update(user);
                _unitOfWork.SaveChanges();
            }
            else 
            {
                user.QuizzesResults.FirstOrDefault(x => x.QuizId == quiz.Id).ResultMark = 0;
                user.QuizzesResults.FirstOrDefault(x => x.QuizId == quiz.Id).JoinedAt = DateTime.Now;
                _unitOfWork.Repository<ApplicationUser>().Update(user);
                _unitOfWork.SaveChanges();
            }
            
            var quizResponseModel = _mapper.Map<QuizResponseModel>(quiz);
            quizResponseModel.Questions = _mapper.Map<List<QuizQuestionResponseModel>>(questions);

            foreach (var question in quizResponseModel.Questions)
            {
                var answers = _unitOfWork.Repository<QuizAnswer>().Get(x => x.QuestionId == question.Id);
                question.Answers = _mapper.Map<List<QuizAnswerResponseModel>>(answers);
            }

            var response = new JoinQuizResponseModel()
            {
                Name = user.UserName,
                CurrentQuiz = _mapper.Map<QuizResponse>(quizResponseModel),
                UserId = user.Id,
            };

            return response;

        }
                
        public async Task<PassQuizResponseModel> PassQuiz(PassQuizRequestModel model)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == _userId)
                                                                .Include(w => w.QuizzesResults)
                                                                    .ThenInclude(w => w.Quiz)
                                                                .Include(w => w.Profile)
                                                                .FirstOrDefault();

            if (user == null)
                throw new CustomException(System.Net.HttpStatusCode.Unauthorized, "Unauthorized", "Unauthorized user cannot access quiz");
            if (user.QuizzesResults.FirstOrDefault(x => x.QuizId == model.QuizId) == null)
                throw new CustomException(System.Net.HttpStatusCode.Forbidden, "user deleted", "user was deleted from quiz");

            var quizSession = user.QuizzesResults.FirstOrDefault(x => x.QuizId == model.QuizId);
            var questions = _unitOfWork.Repository<QuizQuestion>().Get(x => x.QuizId == model.QuizId).ToList();
            var response = new PassQuizResponseModel();


            foreach (var question in questions)
            {
                var usersCorrectAnswers = model.UserAnswers.FindAll(x => x.QuestionId == question.Id && x.IsCorrect == true);
                var correctAnswers = _unitOfWork.Repository<QuizAnswer>().Get(x => x.QuestionId == question.Id && x.IsCorrect == true).ToList();
                if (usersCorrectAnswers.Count == correctAnswers.Count)
                    response.CorrectAnswersCount++;
                else
                    response.InCorrectAnswersCount++;
                                
            }

            response.Name = user.Profile.FullName;
            response.FinalMark = response.CorrectAnswersCount / questions.Count;
            response.ResultId = quizSession.Id;
            quizSession.CountOfCorrectAnswers = response.CorrectAnswersCount;
            quizSession.CountOfIncorrectAnswers = response.InCorrectAnswersCount;
            quizSession.FinishedAt = DateTime.Now;
            _unitOfWork.Repository<UsersResults>().Update(quizSession);
            _unitOfWork.SaveChanges();

            return response;
        }
    }
}
