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
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.Enums;

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

            var quiz = _unitOfWork.Repository<Quiz>().Get(x => x.Id == model.QuizId)
                                                    .Include(w => w.Questions)
                                                        .ThenInclude(w => w.Answers)
                                                    .FirstOrDefault();
            
            if (user == null)
                throw new CustomException(System.Net.HttpStatusCode.Unauthorized, "Unauthorized", "Unauthorized user cannot access quiz");
            if (quiz == null)
                throw new CustomException(System.Net.HttpStatusCode.Forbidden, "invalid quizId", "quiz code is invalid or quiz does not exist");
            if (quiz.IsActive == false)
                throw new CustomException(System.Net.HttpStatusCode.Locked, "quiz closed", "quiz was closed");

            if (user.QuizzesResults.FirstOrDefault(x => x.QuizId == quiz.Id) == null)
            {
                user.QuizzesResults.Add(new UsersResults() {QuizId = quiz.Id, UserId = user.Id, JoinedAt = DateTime.UtcNow, ResultMark = 0 });
                _unitOfWork.Repository<ApplicationUser>().Update(user);
                _unitOfWork.SaveChanges();
            }
            else 
            {
                user.QuizzesResults.FirstOrDefault(x => x.QuizId == quiz.Id).ResultMark = 0;
                user.QuizzesResults.FirstOrDefault(x => x.QuizId == quiz.Id).JoinedAt = DateTime.UtcNow;
                _unitOfWork.Repository<ApplicationUser>().Update(user);
                _unitOfWork.SaveChanges();
            }
            
            var response = new JoinQuizResponseModel()
            {
                Name = user.UserName,
                CurrentQuiz = _mapper.Map<QuizResponse>(quiz),
                UserId = user.Id,
            };

            return response;

        }
                
        public async Task<PassQuizResponseModel> PassQuiz(PassQuizRequestModel model)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == _userId)
                                                                .Include(w => w.QuizzesResults)
                                                                    .ThenInclude(w => w.Quiz)
                                                                        .ThenInclude(w => w.Questions)
                                                                            .ThenInclude(w => w.Answers)
                                                                .Include(w => w.Profile)
                                                                .FirstOrDefault();

            if (user == null)
                throw new CustomException(System.Net.HttpStatusCode.Unauthorized, "Unauthorized", "Unauthorized user cannot access quiz");
            if (user.QuizzesResults.FirstOrDefault(x => x.QuizId == model.QuizId) == null)
                throw new CustomException(System.Net.HttpStatusCode.Forbidden, "user deleted", "user was deleted from quiz");

            var quizSession = user.QuizzesResults.FirstOrDefault(x => x.QuizId == model.QuizId);
            var response = new PassQuizResponseModel();


            foreach (var question in quizSession.Quiz.Questions)
            {
                var usersCorrectAnswers = model.UserAnswers.FindAll(x => x.QuestionId == question.Id && x.IsCorrect);
                var correctAnswers = question.Answers.ToList().FindAll(x => x.IsCorrect);
                if (usersCorrectAnswers.Count == correctAnswers.Count)
                    response.CorrectAnswersCount++;
                else
                    response.InCorrectAnswersCount++;
                                
            }

            response.Name = user.Profile.FullName;
            response.FinalMark = response.CorrectAnswersCount / quizSession.Quiz.Questions.Count;
            response.ResultId = quizSession.Id;
            quizSession.CountOfCorrectAnswers = response.CorrectAnswersCount;
            quizSession.CountOfIncorrectAnswers = response.InCorrectAnswersCount;
            quizSession.FinishedAt = DateTime.UtcNow;
            _unitOfWork.Repository<UsersResults>().Update(quizSession);
            _unitOfWork.SaveChanges();

            return response;
        }

        public PaginationResponseModel<SmallQuizResponseModel> GetAll(PaginationRequestModel<QuizTableColumn> model)
        {
            List<SmallQuizResponseModel> response = new List<SmallQuizResponseModel>();

            var search = !string.IsNullOrEmpty(model.Search) && model.Search.Length > 1;

            var quizzes = _unitOfWork.Repository<Quiz>().Get(x => x.Questions.Count != -1
                                            && (!search || (x.Name.Contains(model.Search) || x.Author.Contains(model.Search) || x.Topic.Name.Contains(model.Search)))
                                            )
                                        .TagWith(nameof(GetAll) + "_GetQuizzes")
                                        .Include(w => w.Topic)
                                        .Include(w => w.Questions)
                                            .ThenInclude(w => w.Answers)
                                        .Select(x => new
                                        {
                                            Name = x.Name,
                                            Author = x.Author,
                                            Topic = x.Topic,
                                            IsActive = x.IsActive,
                                            CreatedAt = x.CreatedAt,
                                            QuestionsCount = x.Questions.Count,
                                            Id = x.Id
                                        });


            if (search)
                quizzes = quizzes.Where(x => x.Name.Contains(model.Search) || x.Author.Contains(model.Search) || x.Topic.Name.Contains(model.Search));

            int count = quizzes.Count();

            if (model.Order != null)
                quizzes = quizzes.OrderBy(model.Order.Key.ToString(), model.Order.Direction == SortingDirection.Asc);

            quizzes = quizzes.Skip(model.Offset).Take(model.Limit);

            response = quizzes.Select(x => new SmallQuizResponseModel
            {
                Name = x.Name,
                Topic = x.Topic.Name,
                Author = x.Author,
                QuestionsCount = x.QuestionsCount,
                CreatedAt = x.CreatedAt,
                IsActive = x.IsActive,
                Id = x.Id

            }).ToList();

            return new(response, count);
        }
        
    }
}
