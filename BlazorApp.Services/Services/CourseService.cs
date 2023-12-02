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
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared.Models.ResponseModel.User;
using Braintree;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services.Services
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

        public async Task AssignCourse(int courseId)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == _userId.Value)
                                                                .Include(w => w.Courses)
                                                                    .ThenInclude(w => w.Course)
                                                                .FirstOrDefault();

            if (user.Courses.Any(x => x.CourseId == courseId))
                throw new CustomException(HttpStatusCode.Forbidden, "invalid courseId", "course already exist in user.Courses");

            var course = _unitOfWork.Repository<Course>().Get(x => x.Id == courseId).FirstOrDefault();

            if (course == null)
                throw new CustomException(HttpStatusCode.BadRequest, "invalid courseId", "course is not found");

            user.Courses.Add(new UsersCourses()
            {
                Course = course,
            });

            _unitOfWork.Repository<ApplicationUser>().Update(user);
            _unitOfWork.SaveChanges();
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

        public PaginationResponseModel<SmallCourseResponseModel> GetAll(PaginationRequestModel<CourseTableColumn> model)
        {
            List<SmallCourseResponseModel> response = new List<SmallCourseResponseModel>();

            var search = !string.IsNullOrEmpty(model.Search) && model.Search.Length > 1;

            var courses = _unitOfWork.Repository<Course>().Get(x => x.ContentURLs.Length > 1
                                            && (!search || (x.Name.Contains(model.Search) || x.Description.Contains(model.Search) || x.Language.Contains(model.Search)))
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
                                            Id = x.Id,
                                            Name = x.Name,
                                            Description = x.Description,
                                            Difficult = x.Difficult,
                                            Language = x.Language,
                                            ContentURLs = x.ContentURLs,
                                            Topic = x.Quizzes.FirstOrDefault().Quiz.Topic.Name,

                                        });


            if (search)
                courses = courses.Where(x => x.Name.Contains(model.Search) || x.Description.Contains(model.Search) || x.Language.Contains(model.Search));

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
                    Topic = course.Topic,
                    Id = course.Id
                };

                response.Add(smallCourseResponseModel);
            }

            return new(response, count);
        }

        public async Task<AssignmentResponseModel> GetAssignment(int courseId)
        {
            var usersCourses = _unitOfWork.Repository<UsersCourses>().Get(x => x.UserId ==_userId.Value && x.CourseId == courseId).Include(w => w.Course)
                                                                                                    .ThenInclude(w => w.Quizzes)
                                                                                                        .ThenInclude(w => w.Quiz)
                                                                                                            .ThenInclude(w => w.Topic)
                                                                                                    .FirstOrDefault();
            var response = new AssignmentResponseModel() 
            {
                Id = courseId,
                ContentURLs = usersCourses.Course.ContentURLs,
                CourseName = usersCourses.Course.Name,
                Language = usersCourses.Course.Language,
                Topics = usersCourses.Course.Quizzes.Select(w => w.Quiz.Topic.Name).ToArray(),
            };

            var userResults = _unitOfWork.Repository<UsersResults>().Get(x => x.UserId == _userId.Value && usersCourses.Course.Quizzes.Select(w => w.QuizId).Any(w => w == x.QuizId))
                                                                        .Include(w => w.Quiz);

            if(userResults != null)
                response.Results = _mapper.Map<List<UserResultResponseModel>>(userResults);

            var nonFoundQuizzesResults = _unitOfWork.Repository<Quiz>().Get(x => usersCourses.Course.Quizzes.Select(w => w.QuizId).Any(w => w == x.Id) && !userResults.Select(w => w.Quiz).Contains(x)).Select(w => w.Id);

            response.NotFinishedQuizzesIds = nonFoundQuizzesResults.ToList();

            return response;

        }

        public PaginationResponseModel<SmallCourseResponseModel> GetAssignments(PaginationRequestModel<CourseTableColumn> model)
        {
            List<SmallCourseResponseModel> response = new List<SmallCourseResponseModel>();

            var search = !string.IsNullOrEmpty(model.Search) && model.Search.Length > 1;

            var courses = _unitOfWork.Repository<ApplicationUser>()
                             .Get(x => x.Courses.Any(w => w.Course.ContentURLs.Length > 1) &&
                                 (!search || x.Courses.Any(c => c.Course.Name.Contains(model.Search) ||
                                     c.Course.Description.Contains(model.Search) ||
                                     c.Course.Language.Contains(model.Search))))
                             .Include(x => x.Courses)
                                .ThenInclude(x => x.Course)
                                    .ThenInclude(x => x.Quizzes)
                                        .ThenInclude(x => x.Quiz)
                                            .ThenInclude(x => x.Topic)
                             .SelectMany(x => x.Courses.Select(c => c.Course))
                             .Select(course => new
                             {
                                 Id = course.Id,
                                 Name = course.Name,
                                 Description = course.Description,
                                 Difficult = course.Difficult,
                                 Language = course.Language,
                                 ContentURLs = course.ContentURLs,
                                 Topic = course.Quizzes.Select(w => w.Quiz).FirstOrDefault().Topic.Name
                             });


            if (search)
                courses = courses.Where(x => x.Name.Contains(model.Search) || x.Description.Contains(model.Search) || x.Language.Contains(model.Search));

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
                    Topic = course.Topic,
                    Id = course.Id
                };

                response.Add(smallCourseResponseModel);
            }

            return new(response, count);
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

        public async Task<CourseResponseModel> UpdateCourse(CourseRequestModel model, int id)
        {
            var course = _unitOfWork.Repository<Course>().Get(x => x.Id == id)
                                                            .Include(w => w.Quizzes)
                                                                .ThenInclude(w => w.Quiz)
                                                                    .ThenInclude(w => w.Topic)
                                                        .FirstOrDefault();

            if (course == null)
                throw new CustomException(HttpStatusCode.BadRequest, "invalid id", "course is not found");

            course.Name = model.Name;
            course.Description = model.Description;
            course.Difficult = model.Difficult;
            course.Language = model.Language;
            course.ContentURLs = model.ContentURLs;

            var updQuizzes = new List<CoursesQuizzes>();
            model.QuizzesIds.ToList().ForEach(x =>
            {
                if (!updQuizzes.Select(w => w.QuizId).Contains(x)) 
                {
                    updQuizzes.Add(new CoursesQuizzes()
                    {
                        QuizId = x,
                    });
                }
            });

            course.Quizzes = updQuizzes.ToList();

            _unitOfWork.Repository<Course>().Update(course);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<CourseResponseModel>(course);
            return response;
        }

        
    }
}
