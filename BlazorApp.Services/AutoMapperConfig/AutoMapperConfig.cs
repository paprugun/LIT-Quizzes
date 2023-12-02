using BlazorApp.Common.Extensions;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Session;
using Profile = BlazorApp.Domain.Entities.Identity.Profile;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.Models.RequestModels.Quiz;
using BlazorApp.Models.ResponseModels.Course;
using BlazorApp.Models.RequestModels.Course;
using System.Linq;
using BlazorApp.Shared.Models.ResponseModels.Quiz;

namespace ApplicationAuth.Services.StartApp
{
    public class AutoMapperProfileConfiguration : AutoMapper.Profile
    {
        public AutoMapperProfileConfiguration()
        : this("MyProfile")
        {
        }

        protected AutoMapperProfileConfiguration(string profileName)
        : base(profileName)
        {

            #region Quiz models
            CreateMap<Quiz, QuizResponseModel>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(t => t.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(t => t.Topic, opt => opt.MapFrom(src => src.Topic))
                .ForMember(t => t.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(t => t.Questions, opt => opt.MapFrom(src => src.Questions))
                .ForMember(t => t.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToISO()));

            CreateMap<Quiz, QuizResponse>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(t => t.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(t => t.Topic, opt => opt.MapFrom(src => src.Topic))
                .ForMember(t => t.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(t => t.Questions, opt => opt.MapFrom(src => src.Questions))
                .ForMember(t => t.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToISO()));

            CreateMap<Quiz, SmallQuizResponseModel>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(t => t.QuestionsCount, opt => opt.MapFrom(src => src.Questions.Count))
                .ForMember(t => t.Topic, opt => opt.MapFrom(src => src.Topic.Name))
                .ForMember(t => t.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(t => t.Author, opt => opt.MapFrom(src => src.Author));

            CreateMap<QuizQuestion, QuizQuestionResponseModel>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(t => t.QuizId, opt => opt.MapFrom(src => src.Quiz.Id))
                .ForMember(t => t.Answers, opt => opt.MapFrom(src => src.Answers))
                .ForMember(t => t.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToISO()));

            CreateMap<QuizQuestion, QuizQuestionResponse>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(t => t.QuizId, opt => opt.MapFrom(src => src.Quiz.Id))
                .ForMember(t => t.Answers, opt => opt.MapFrom(src => src.Answers))
                .ForMember(t => t.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToISO()));

            CreateMap<QuizAnswer, QuizAnswerResponseModel>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(t => t.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
                .ForMember(t => t.QuestionId, opt => opt.MapFrom(src => src.Question.Id))
                .ForMember(t => t.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToISO()));

            CreateMap<QuizAnswer, QuizAnswerResponse>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(t => t.IsCorrect, opt => opt.MapFrom(src => src.IsCorrect))
                .ForMember(t => t.QuestionId, opt => opt.MapFrom(src => src.Question.Id))
                .ForMember(t => t.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToISO()));




            #endregion
            #region ClientModels To ServerModels
            #region Requests
            #endregion
            #endregion
            #region Topics models
            CreateMap<Topic, TopicResponseModel>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Name));

            #endregion
            #region Results models
            CreateMap<UsersResults, SmallUserResultResponseModel>()
                .ForMember(t => t.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(t => t.QuizId, opt => opt.MapFrom(x => x.QuizId))
                .ForMember(t => t.QuizName, opt => opt.MapFrom(x => x.Quiz.Name))
                .ForMember(t => t.ResultMark, opt => opt.MapFrom(x => x.ResultMark))
                .ForMember(t => t.CountOfCorrectAnswers, opt => opt.MapFrom(x => x.CountOfCorrectAnswers))
                .ForMember(t => t.CountOfIncorrectAnswers, opt => opt.MapFrom(x => x.CountOfIncorrectAnswers))
                .ForMember(t => t.JoinedAt, opt => opt.MapFrom(x => x.JoinedAt));
            CreateMap<UsersResults, UserResultResponseModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(x => x.Quiz, opt => opt.MapFrom(x => x.Quiz))
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.User.Profile.FullName))
                .ForMember(x => x.ResultMark, opt => opt.MapFrom(x => x.ResultMark))
                .ForMember(x => x.CountOfCurrentAnswers, opt => opt.MapFrom(x => x.CountOfCorrectAnswers))
                .ForMember(x => x.CountOfIncorrectAnswers, opt => opt.MapFrom(x => x.CountOfIncorrectAnswers))
                .ForMember(x => x.JoinedAt, opt => opt.MapFrom(x => x.JoinedAt));
            #endregion
            CreateMap<UserDevice, UserDeviceResponseModel>()
                .ForMember(t => t.AddedAt, opt => opt.MapFrom(src => src.AddedAt.ToISO()));

            #region User model

            CreateMap<UserProfileRequestModel, Profile>()
                .ForMember(t => t.Id, opt => opt.Ignore())
                .ForMember(t => t.User, opt => opt.Ignore());

			CreateMap<Profile, BlazorApp.Models.ResponseModels.UserResponseModel>()
                .ForMember(t => t.Email, opt => opt.MapFrom(x => x.User != null ? x.User.Email : ""))
                .ForMember(t => t.PhoneNumber, opt => opt.MapFrom(x => x.User != null ? x.User.PhoneNumber : ""))
                .ForMember(t => t.IsBlocked, opt => opt.MapFrom(x => x.User != null ? !x.User.IsActive : false));

            CreateMap<ApplicationUser, UserBaseResponseModel>()
               .IncludeAllDerived();

			CreateMap<ApplicationUser, BlazorApp.Models.ResponseModels.UserResponseModel>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.Profile.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.Profile.LastName))
                .ForMember(x => x.IsBlocked, opt => opt.MapFrom(x => !x.IsActive))
                .IncludeAllDerived();

			CreateMap<ApplicationUser, BlazorApp.Models.ResponseModels.Session.UserRoleResponseModel>();

            #endregion
            #region Course model
            CreateMap<Course, CourseResponseModel>()
                .ForMember(t => t.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(t => t.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(t => t.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(t => t.ContentURLs, opt => opt.MapFrom(x => x.ContentURLs))
                .ForMember(t => t.Difficult, opt => opt.MapFrom(x => x.Difficult))
                .ForMember(t => t.Language, opt => opt.MapFrom(x => x.Language))
                .ForMember(t => t.Topics, opt => opt.MapFrom(x => x.Quizzes.Select(c => c.Quiz.Topic.Name)))
                .ForMember(t => t.Quizzes, opt => opt.MapFrom(x => x.Quizzes.Select(c => c.QuizId)));

            CreateMap<CourseRequestModel, Course>()
                .ForMember(t => t.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(t => t.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(t => t.ContentURLs, opt => opt.MapFrom(x => x.ContentURLs))
                .ForMember(t => t.Difficult, opt => opt.MapFrom(x => x.Difficult))
                .ForMember(t => t.Language, opt => opt.MapFrom(x => x.Language));

            CreateMap<Course, SmallCourseResponseModel>()
               .ForMember(t => t.Id, opt => opt.MapFrom(x => x.Id))
               .ForMember(t => t.Name, opt => opt.MapFrom(x => x.Name))
               .ForMember(t => t.Difficult, opt => opt.MapFrom(x => x.Difficult))
               .ForMember(t => t.Topic, opt => opt.MapFrom(x => x.Quizzes.Select(c => c.Quiz.Topic.Name).FirstOrDefault()))
               .ForMember(t => t.Language, opt => opt.MapFrom(x => x.Language));

            #endregion
        }
    }
}
