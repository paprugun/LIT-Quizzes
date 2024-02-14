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

            /*
            var courseLessons = new List<CourseLesson>();
            var quizzesIds = new List<int>();
            model.Lessons.ForEach(w => quizzesIds.AddRange(w.QuizzesIds));
            var quizzes = _unitOfWork.Repository<Quiz>().Get(x => quizzesIds.Contains(x.Id)).Include(w => w.Topic);

            if (quizzes.Any(x => x == null))
                throw new CustomException(HttpStatusCode.BadRequest, "quizzes are null", "quizzes are not found");
            model.Lessons.ForEach(w =>
            {
                courseLessons.Add(new CourseLesson()
                {
                    Name = w.Name,
                    Description = w.Description,
                    CourseId = w.CourseId,
                    URL = w.URL,
                    Time = w.Time,
                });
            });

            quizzes.ToList().ForEach(w =>
            {
                courseLessons.ForEach(c =>
                {
                    c.Quizzes.Add(new LessonQuizzes()
                    {
                        LessonId = c.Id,
                        Quiz = w,
                    });
                });
            });
            */
            var topic = _unitOfWork.Repository<Topic>().Get(x => x.Name == model.Topic).FirstOrDefault();

            if(topic == null)
                throw new CustomException(HttpStatusCode.BadRequest, "topic not found", "Topic is not found");

            var course = new Course()
            {
                Name = model.Name,
                Description = model.Description,
                Difficult = model.Difficult,
                Language = model.Language,
                Topic = topic,
            };

            _unitOfWork.Repository<Course>().Insert(course);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<CourseResponseModel>(course);
            return response;

        }

        public async Task<LessonResponseModel> CreateLesson(LessonRequestModel model)
        {
            var course = _unitOfWork.Repository<Course>().Get(x => x.Id == model.CourseId)
                                                        .Include(w => w.Lessons)
                                                            .ThenInclude(w => w.Quizzes)
                                                        .FirstOrDefault();

            if (course == null)
                throw new CustomException(HttpStatusCode.BadRequest, "invalid courseId", "Course is not found");

            var lesson = new CourseLesson()
            {
                Name = model.Name,
                Description = model.Description,
                CourseId = model.CourseId,
                Time = model.Time,
                URL = model.URL,
            };

            _unitOfWork.Repository<CourseLesson>().Insert(lesson);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<LessonResponseModel>(lesson);
            return response;
        }

        public async Task<LessonResponseModel> UpdateLesson(LessonRequestModel model, int id)
        {
            var lesson = _unitOfWork.Repository<CourseLesson>().Get(x => x.Id == id)
                                                                .Include(w => w.Quizzes)
                                                                    .ThenInclude(w=>w.Quiz)
                                                                .FirstOrDefault();

            if (lesson == null)
                throw new CustomException(HttpStatusCode.BadRequest, "invalid lesson id", "lesson is not found");

            var quizzes = _unitOfWork.Repository<Quiz>().Get(x => model.QuizzesIds.Contains(x.Id));

            if (quizzes.Any(x => x == null))
                throw new CustomException(HttpStatusCode.BadRequest, "invalid quizzes ids", "quiz is not found");

            if (model.QuizzesIds.Count() < lesson.Quizzes.Count)
            {
                var idsToDel = lesson.Quizzes.Select(w => w.QuizId).Except(model.QuizzesIds);
                foreach (var item in lesson.Quizzes.Where(w => idsToDel.Contains(w.QuizId)))
                {
                    lesson.Quizzes.Remove(item);
                }
            }

            if (model.QuizzesIds.Count() > lesson.Quizzes.Count)
            {
                var idsToAdd = model.QuizzesIds.Except(lesson.Quizzes.Select(w => w.QuizId));
                quizzes = _unitOfWork.Repository<Quiz>().Get(x => idsToAdd.Contains(x.Id));
                foreach (var item in quizzes)
                {
                    lesson.Quizzes.Add(new LessonQuizzes()
                    {
                        Quiz = item,
                    });
                }
            }

            _unitOfWork.Repository<CourseLesson>().Update(lesson);
            _unitOfWork.SaveChanges();

            var response = _mapper.Map<LessonResponseModel>(lesson);
            lesson.Quizzes.ToList().ForEach(x => 
            {
                response.Quizzes.Add(new(x.QuizId, x.Quiz.Name));
            });
            return response;
        }

        public async Task DeleteCourse(int id)
        {
            _unitOfWork.Repository<Course>().DeleteById(id);
            _unitOfWork.SaveChanges();
        }

        public async Task DeleteLesson(int id)
        {
            var lesson = _unitOfWork.Repository<CourseLesson>().Get(x => x.Id == id).FirstOrDefault();

            if (lesson == null)
                throw new CustomException(HttpStatusCode.BadRequest, "invalid lesson id", "Lesson is not found");

            _unitOfWork.Repository<CourseLesson>().DeleteById(id);
            _unitOfWork.SaveChanges();
        }

        public async Task<AdminCourseResponseModel> GetAdminCourse(int id)
        {
            var course = _unitOfWork.Repository<Course>().Get(x => x.Id == id)
                                                        .Include(w => w.Lessons)
                                                        .ThenInclude(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                .ThenInclude(w => w.Topic)
                                                         .Include(w => w.Lessons)
                                                            .ThenInclude(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                    .ThenInclude(w => w.Questions)
                                                                        .ThenInclude(w => w.Answers)
                                                          .Include(w => w.Lessons)
                                                            .ThenInclude(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                .ThenInclude(w => w.Users)
                                                                    .ThenInclude(w => w.User)
                                                                        .ThenInclude(w => w.QuizzesResults)
                                                            .Include(w => w.Lessons)
                                                                .ThenInclude(w => w.Quizzes)
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

            response.Lessons.ForEach(lesson => {
                lesson.Quizzes = course.Lessons
                    .Where(w => w.Id == lesson.Id)
                    .SelectMany(w => w.Quizzes)
                    .Select(c => (c.QuizId, c.Quiz.Name))
                    .ToList();
            });

            response.UsersResults = new List<UserCourseResultResponseModel>();
            foreach (var result in course.Users)
            {
                var courseResult = new UserCourseResultResponseModel();
                courseResult.Id = result.Id;
                courseResult.CourseId = course.Id;
                courseResult.CourseName = course.Name;
                courseResult.UserId = result.UserId;
                courseResult.UserName = result.User.UserName;
                courseResult.CountOfDoneSteps = result.User.QuizzesResults.Where(w => course.Lessons.SelectMany(c => c.Quizzes.Select(q => q.QuizId)).Contains(w.QuizId)).Count();
                courseResult.CountOfLeftSteps = course.Lessons.Select(w => w.Quizzes).Count() - courseResult.CountOfDoneSteps;
                response.UsersResults.Add(courseResult);
            }
            return response;
        }

        public PaginationResponseModel<SmallCourseResponseModel> GetAll(PaginationRequestModel<CourseTableColumn> model)
        {
            List<SmallCourseResponseModel> response = new List<SmallCourseResponseModel>();
            var search = !string.IsNullOrEmpty(model.Search) && model.Search.Length > 1;
            var filter = model.Filters != null && model.Filters.Count > 0;

            var courses = _unitOfWork.Repository<Course>().Get(x => filter == false || model.Filters.Any(c => x.Topic.Name.Contains(c)) || model.Filters.Contains(x.Difficult.ToString()) || model.Filters.Any(c => c == x.Language) || model.Filters.Any(c => c.Equals(x.Difficult.ToString())))
                                        .TagWith(nameof(GetAll) + "_GetCourses")
                                        .Include(w => w.Lessons)
                                        .ThenInclude(w => w.Quizzes)
                                            .ThenInclude(w => w.Quiz)
                                                .ThenInclude(w => w.Questions)
                                                    .ThenInclude(w => w.Answers)
                                        .Include(w => w.Lessons)
                                            .ThenInclude(w => w.Quizzes)
                                            .ThenInclude(w => w.Quiz)
                                                .ThenInclude(w => w.Topic)
                                        .Include(w => w.Topic)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.Name,
                                            x.Description,
                                            x.Difficult,
                                            x.Language,
                                            x.Lessons,
                                            Topic = x.Topic.Name,
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
                    LessonsCount = course.Lessons.Count,
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
                                                        .Include(w => w.Lessons)
                                                            .ThenInclude(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                .ThenInclude(w => w.Topic)
                                                         .Include(w => w.Lessons)
                                                         .ThenInclude(w => w.Quizzes)
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
                                                            .Include(w => w.Lessons)
                                                            .ThenInclude(w => w.Quizzes)
                                                                .ThenInclude(w => w.Quiz)
                                                                    .ThenInclude(w => w.Questions)
                                                                        .ThenInclude(w => w.Answers)
                                                          .Include(w => w.Lessons)
                                                            .ThenInclude(w => w.Quizzes)
                                                            .ThenInclude(w => w.Quiz)
                                                                .ThenInclude(w => w.Users)
                                                                    .ThenInclude(w => w.User)
                                                                        .ThenInclude(w => w.QuizzesResults)
                                                            .Include(w => w.Lessons)
                                                            .ThenInclude(w => w.Quizzes)
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

            var courseLessons = new List<CourseLesson>();
            var quizzesIds = new List<int>();
            model.Lessons.ForEach(w => quizzesIds.AddRange(w.QuizzesIds));

            var quizzes = _unitOfWork.Repository<Quiz>().Get(x => quizzesIds.Contains(x.Id)).Include(w => w.Topic);

            if (quizzes.Any(x => x == null))
                throw new CustomException(HttpStatusCode.BadRequest, "quizzes are null", "quizzes are not found");

            model.Lessons.ForEach(w =>
            {
                courseLessons.Add(new CourseLesson()
                {
                    Name = w.Name,
                    Description = w.Description,
                    CourseId = w.CourseId,
                    URL = w.URL,
                    Time = w.Time,
                });
            });

            quizzes.ToList().ForEach(w =>
            {
                courseLessons.ForEach(c =>
                {
                    c.Quizzes.Add(new LessonQuizzes()
                    {
                        LessonId = c.Id,
                        Quiz = w,
                    });
                });
            });

            course.Lessons = courseLessons;

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
                courseResult.CountOfDoneSteps = result.User.QuizzesResults.Where(w => course.Lessons.SelectMany(c => c.Quizzes.Select(q => q.QuizId)).Contains(w.QuizId)).Count();
                courseResult.CountOfLeftSteps = course.Lessons.Select(w => w.Quizzes).Count() - courseResult.CountOfDoneSteps;
                response.UsersResults.Add(courseResult);
            }
            return response;
        }


    }
}
