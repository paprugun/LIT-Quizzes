using AutoMapper;
using BlazorApp.Common.Exceptions;
using BlazorApp.Common.Extensions;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.RequestModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels.Course;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.Services.Interfaces.CourseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Services.CourseServices
{
    public class UserCourseService : IUserCourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private bool _isUserSuperAdmin = false;
        private bool _isUserAdmin = false;
        private int? _userId = null;

        public UserCourseService(IUnitOfWork unitOfWork,
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
        public async Task<AssignmentResponseModel> GetAssignment(int userCourseId)
        {
            /*var usersCourses = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == _userId)
                                                                        .Include(w => w.Profile)
                                                                        .Include(w => w.Courses)
                                                                            .ThenInclude(w => w.Course)
                                                                                .ThenInclude(w => w.Quizzes)
                                                                                    .ThenInclude(w => w.Quiz)
                                                                                        .ThenInclude(w => w.Topic)
                                                                        .FirstOrDefault().Courses
                                                                        .FirstOrDefault(x => x.CourseId == courseId);*/

            var courses = _unitOfWork.Repository<UsersCourses>().Get(x => x.Id == userCourseId).ToList();

            var usersCourses = _unitOfWork.Repository<UsersCourses>().Get(x => x.UserId == _userId.Value && x.Id == userCourseId)
                                                                                                    .Include(w => w.Course)
                                                                                                        .ThenInclude(w => w.Quizzes)
                                                                                                            .ThenInclude(w => w.Quiz)
                                                                                                                .ThenInclude(w => w.Topic)
                                                                                                    .Include(w => w.User)
                                                                                                        .ThenInclude(w => w.Profile)
                                                                                                    .FirstOrDefault();
            var response = new AssignmentResponseModel()
            {
                Id = userCourseId,
                ContentURLs = usersCourses.Course.ContentURLs,
                CourseName = usersCourses.Course.Name,
                Language = usersCourses.Course.Language,
                Topics = usersCourses.Course.Quizzes.Select(w => w.Quiz.Topic.Name).ToArray(),
            };

            var userResults = _unitOfWork.Repository<UsersResults>().Get(x => x.UserId == _userId.Value && usersCourses.Course.Quizzes.Select(w => w.QuizId).Any(w => w == x.QuizId))
                                                                        .Include(w => w.Quiz);

            if (userResults != null)
                response.Results = _mapper.Map<List<SmallUserResultResponseModel>>(userResults);

            var nonFoundQuizzesResults = _unitOfWork.Repository<Quiz>().Get(x => usersCourses.Course.Quizzes.Select(w => w.QuizId).Any(w => w == x.Id) && !userResults.Select(w => w.Quiz).Contains(x)).Include(w => w.Topic).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
            });

            response.NotFinishedQuizzes = nonFoundQuizzesResults.AsEnumerable().Select(x => ((int)x.Id, (string)x.Name)).ToList();

            return response;
        }
        public PaginationResponseModel<SmallCourseResponseModel> GetAssignments(PaginationRequestModel<CourseTableColumn> model)
        {
            List<SmallCourseResponseModel> response = new List<SmallCourseResponseModel>();

            var search = !string.IsNullOrEmpty(model.Search) && model.Search.Length > 1;

            var courses = _unitOfWork.Repository<ApplicationUser>()
                             .Get(x => x.Id == _userId.Value && x.Courses.Any(w => w.Course.ContentURLs.Length > 1) &&
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
                                 course.Id,
                                 course.Name,
                                 course.Description,
                                 course.Difficult,
                                 course.Language,
                                 course.ContentURLs,
                                 Topic = course.Quizzes.Select(w => w.Quiz).FirstOrDefault().Topic.Name
                             });


            if (search)
                courses = courses.Where(x => x.Name.Contains(model.Search) || x.Description.Contains(model.Search) || x.Language.Contains(model.Search));

            int count = courses.Count();

            courses = courses.Skip(model.Offset).Take(model.Limit);

            foreach (var course in courses)
            {
                //поменять модельку на юзерКурсРезалтРеспонсмодел
                SmallCourseResponseModel smallCourseResponseModel = new SmallCourseResponseModel
                {
                    Name = course.Name,
                    Difficult = (int)course.Difficult,
                    Language = course.Language,
                    LessonsCount = course.ContentURLs?.Split(" ").Length ?? 0,
                    Topic = course.Topic,
                    Id = course.Id
                };

                response.Add(smallCourseResponseModel);
            }

            return new(response, count);
        }

        public CursorPaginationBaseResponseModel<Models.ResponseModels.Course.UserCourseResultResponseModel> GetAssignments(CursorPaginationRequestModel<CourseTableColumn> model)
        {
            var search = !string.IsNullOrEmpty(model.Search) && model.Search.Length > 1;

            var courses = _unitOfWork.Repository<ApplicationUser>()
                             .Get(x => x.Id == _userId.Value && x.Courses.Any(w => w.Course.ContentURLs.Length > 1) &&
                                 (!search || x.Courses.Any(c => c.Course.Name.Contains(model.Search) ||
                                     c.Course.Description.Contains(model.Search) ||
                                     c.Course.Language.Contains(model.Search))))
                             .Include(x => x.Courses)
                                .ThenInclude(x => x.Course)
                                    .ThenInclude(x => x.Quizzes)
                                        .ThenInclude(x => x.Quiz)
                                            .ThenInclude(x => x.Topic)
                             .SelectMany(x => x.Courses)
                             .Select(userCourse => new UserCourseResultResponseModel
                             {
                                 Id = userCourse.Id,
                                 CourseName = userCourse.Course.Name,
                                 CourseId = userCourse.CourseId,
                                 UserId = _userId.Value,
                                 UserName = userCourse.User.UserName,
                                 CountOfDoneSteps = userCourse.User.QuizzesResults.Where(w => userCourse.Course.Quizzes.Select(c => c.QuizId).Contains(w.QuizId)).Count(),
                                 CountOfLeftSteps = userCourse.Course.Quizzes.Count() - userCourse.User.QuizzesResults.Where(w => userCourse.Course.Quizzes.Select(c => c.QuizId).Contains(w.QuizId)).Count()
                             });

            if (search)
                courses = courses.Where(x => x.CourseName.Contains(model.Search) || x.UserName.Contains(model.Search));

            if (model.Order != null)
                courses = courses.OrderBy(model.Order.Key.ToString(), model.Order.Direction == SortingDirection.Asc);

            var courseList = courses.ToList();

            var offset = 0;

            if (model.LastId.HasValue)
            {
                var item = courseList.FirstOrDefault(u => u.Id == model.LastId);

                if (item is null)
                    throw new CustomException(HttpStatusCode.BadRequest, "lastId", "There is no assigment with specific id in selection");

                offset = courseList.IndexOf(item) + 1;
            }

            courses = courses.Skip(offset).Take(model.Limit + 1);

            var response = courses;

            int? nextCursorId = null;

            if (courses.Count() > model.Limit)
            {
                response = response.Take(model.Limit);
                nextCursorId = response.AsEnumerable().LastOrDefault()?.Id;
            }

            return new(response.ToList(), nextCursorId);
        }
    }
}
