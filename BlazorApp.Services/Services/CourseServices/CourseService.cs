using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlazorApp.Common.Exceptions;
using BlazorApp.Common.Extensions;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.RequestModels.Course;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Course;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.Services.Interfaces.CourseServices;
using Braintree;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services.Services.CourseServices
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private bool _isUserSuperAdmin = false;
        private bool _isUserAdmin = false;
        private int? _userId = null;

        public CourseService(IUnitOfWork unitOfWork,
                            IMapper mapper,
                            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

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

        public async Task<CourseResponseModel> CreateCourse(CourseRequestModel model)
        {
            var courseQuizzes = new List<CoursesQuizzes>();
            var quizzes = _unitOfWork.Repository<Quiz>().Get(x => model.QuizzesIds.Contains(x.Id)).Include(w => w.Topic);

            if (quizzes == null)
                throw new CustomException(HttpStatusCode.BadRequest, "quizzes are null", "quizzes are not found");

            var list = quizzes.ToList();

            model.QuizzesIds.ToList().ForEach(x =>
            {
                courseQuizzes.Add(new CoursesQuizzes()
                {
                    Quiz = quizzes.FirstOrDefault(w => w.Id == x),
                });
            });
            var course = new Course()
            {
                Name = model.Name,
                Description = model.Description,
                Difficult = model.Difficult,
                ContentURLs = model.ContentURLs,
                Language = model.Language,
                Quizzes = courseQuizzes
            };

            _unitOfWork.Repository<Course>().Insert(course);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<CourseResponseModel>(course);
            return response;

        }

        public async Task DeleteCourse(int id)
        {
            _unitOfWork.Repository<Course>().DeleteById(id);
            _unitOfWork.SaveChanges();
        }

        public async Task<AdminCourseResponseModel> GetAdminCourse(int id)
        {
            var course = _unitOfWork.Repository<Course>().Get(x => x.Id == id)
                                                        .Include(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                .ThenInclude(w => w.Topic)
                                                         .Include(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                    .ThenInclude(w => w.Questions)
                                                                        .ThenInclude(w => w.Answers)
                                                          .Include(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                .ThenInclude(w => w.Users)
                                                                    .ThenInclude(w => w.User)
                                                                        .ThenInclude(w => w.QuizzesResults)
                                                            .Include(w => w.Quizzes)
                                                                .ThenInclude(w => w.Quiz)
                                                                    .ThenInclude(w => w.Users)
                                                                        .ThenInclude(w => w.User)
                                                                .ThenInclude(w => w.Profile)
                                                            .Include(w => w.Users)
                                                                .ThenInclude(w => w.User)
                                                                    .ThenInclude(w => w.QuizzesResults)
                                                        .FirstOrDefault();
            if (course == null)
                throw new CustomException(HttpStatusCode.BadRequest, "invalid id", "course is not found");

            var response = _mapper.Map<AdminCourseResponseModel>(course);
            response.UsersResults = new List<UserCourseResultResponseModel>();
            foreach (var result in course.Users)
            {
                var courseResult = new UserCourseResultResponseModel();
                courseResult.Id = result.Id;
                courseResult.CourseId = course.Id;
                courseResult.CourseName = course.Name;
                courseResult.UserId = result.UserId;
                courseResult.UserName = result.User.UserName;
                courseResult.CountOfDoneSteps = result.User.QuizzesResults.ToList().FindAll(x => course.Quizzes.Select(c => c.QuizId).Contains(x.QuizId)).Count();
                courseResult.CountOfLeftSteps = course.Quizzes.Count() - courseResult.CountOfDoneSteps;
                response.UsersResults.Add(courseResult);
            }
            return response;
        }

        public PaginationResponseModel<SmallCourseResponseModel> GetAll(PaginationRequestModel<CourseTableColumn> model)
        {
            List<SmallCourseResponseModel> response = new List<SmallCourseResponseModel>();
            var search = !string.IsNullOrEmpty(model.Search) && model.Search.Length > 1;
            var filter = model.Filters != null && model.Filters.Count > 0;

            var courses = _unitOfWork.Repository<Course>().Get(x => x.ContentURLs.Length > 1
                                                                  && (filter == false || x.Quizzes.Select(w => w.Quiz.Topic.Name).All(c => model.Filters.Contains(c) || model.Filters.Any(c => c == x.Language) || model.Filters.Any(c => c.Equals(x.Difficult.ToString()))))
                                                              )
                                        .TagWith(nameof(GetAll) + "_GetCourses")
                                        .Include(w => w.Quizzes)
                                            .ThenInclude(w => w.Quiz)
                                                .ThenInclude(w => w.Questions)
                                                    .ThenInclude(w => w.Answers)
                                        .Include(w => w.Quizzes)
                                            .ThenInclude(w => w.Quiz)
                                                .ThenInclude(w => w.Topic)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.Name,
                                            x.Description,
                                            x.Difficult,
                                            x.Language,
                                            x.ContentURLs,
                                            Topics = x.Quizzes.Select(w => w.Quiz.Topic.Name),
                                        });
            if (search)
                courses = courses.Where(x => x.Name.Contains(model.Search) || x.Description.Contains(model.Search));

            int count = courses.Count();

            courses = courses.Skip(model.Offset).Take(model.Limit);

            foreach (var course in courses)
            {
                SmallCourseResponseModel smallCourseResponseModel = new SmallCourseResponseModel
                {
                    Name = course.Name,
                    Difficult = course.Difficult,
                    Language = course.Language,
                    LessonsCount = course.ContentURLs?.Split(" ").Length ?? 0,
                    Topic = course.Topics.Max(),
                    Id = course.Id
                };

                response.Add(smallCourseResponseModel);
            }

            return new(response, count);
        }

        public async Task<CourseResponseModel> GetCourse1(int id)
        {
            var course = _unitOfWork.Repository<Course>().Get(x => x.Id == id)
                                                        .Include(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                .ThenInclude(w => w.Topic)
                                                         .Include(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                    .ThenInclude(w => w.Questions)
                                                                        .ThenInclude(w => w.Answers)
                                                        .Include(w => w.Users)
                                                            .ThenInclude(w => w.User)
                                                                .ThenInclude(w => w.QuizzesResults)
                                                        .FirstOrDefault();
            if (course == null)
                throw new CustomException(HttpStatusCode.BadRequest, "invalid id", "course is not found");

            var courseResults = new List<UserCourseResultResponseModel>();

            foreach (var user in course.Users.Select(w => w.User))
            {
                var courseResult = new UserCourseResultResponseModel();
                courseResult.Id = user.Id;
                courseResult.CourseId = course.Id;
                courseResult.CountOfDoneSteps = user.QuizzesResults.ToList().FindAll(x => course.Quizzes.Select(c => c.QuizId).Contains(x.QuizId)).Count();
                courseResults.Add(courseResult);
            }


            var response = _mapper.Map<CourseResponseModel>(course);
            //response.UsersResults = courseResults;
            return response;
        }

        public async Task<CourseResponseModel> GetCourse(int id)
        {
            var course = _unitOfWork.Repository<Course>().Get(x => x.Id == id)
                                                        .Include(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                .ThenInclude(w => w.Topic)
                                                         .Include(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                    .ThenInclude(w => w.Questions)
                                                                        .ThenInclude(w => w.Answers)
                                                        .FirstOrDefault();
            if (course == null)
                throw new CustomException(HttpStatusCode.BadRequest, "invalid id", "course is not found");

            var response = _mapper.Map<CourseResponseModel>(course);
            return response;
        }

        public async Task<AdminCourseResponseModel> UpdateCourse(CourseRequestModel model, int id)
        {
            var course = _unitOfWork.Repository<Course>().Get(x => x.Id == id)
                                                            .Include(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                .ThenInclude(w => w.Topic)
                                                         .Include(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                    .ThenInclude(w => w.Questions)
                                                                        .ThenInclude(w => w.Answers)
                                                          .Include(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                .ThenInclude(w => w.Users)
                                                                    .ThenInclude(w => w.User)
                                                                        .ThenInclude(w => w.QuizzesResults)
                                                            .Include(w => w.Quizzes)
                                                                .ThenInclude(w => w.Quiz)
                                                                    .ThenInclude(w => w.Users)
                                                                        .ThenInclude(w => w.User)
                                                                .ThenInclude(w => w.Profile)
                                                            .Include(w => w.Users)
                                                                .ThenInclude(w => w.User)
                                                                    .ThenInclude(w => w.QuizzesResults)
                                                        .FirstOrDefault();

            if (course == null)
                throw new CustomException(HttpStatusCode.BadRequest, "invalid id", "course is not found");

            course.Name = model.Name;
            course.Description = model.Description;
            course.Difficult = model.Difficult;
            course.Language = model.Language;
            course.ContentURLs = model.ContentURLs;

            var quizzes = new List<CoursesQuizzes>();

            var quizzesData = _unitOfWork.Repository<Quiz>().Get(x => model.QuizzesIds.Contains(x.Id)).Include(w => w.Topic);
            quizzesData.ToList().ForEach(w =>
            {
                quizzes.Add(new CoursesQuizzes { CourseId = id, QuizId = w.Id, Quiz = w, });
            });
            course.Quizzes = quizzes;

            _unitOfWork.Repository<Course>().Update(course);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<AdminCourseResponseModel>(course);

            response.UsersResults = new List<UserCourseResultResponseModel>();
            foreach (var result in course.Users)
            {
                var courseResult = new UserCourseResultResponseModel();
                courseResult.Id = result.Id;
                courseResult.CourseId = course.Id;
                courseResult.CourseName = course.Name;
                courseResult.UserId = result.UserId;
                courseResult.UserName = result.User.UserName;
                courseResult.CountOfDoneSteps = result.User.QuizzesResults.ToList().FindAll(x => course.Quizzes.Select(c => c.QuizId).Contains(x.QuizId)).Count();
                courseResult.CountOfLeftSteps = course.Quizzes.Count() - courseResult.CountOfDoneSteps;
                response.UsersResults.Add(courseResult);
            }
            response.Topics = response.Topics.Distinct().ToArray();
            return response;
        }


    }
}
