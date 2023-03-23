using BlazorApp.Common.Extensions;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Shared.Models.ResponseModels.Quiz;
using BlazorApp.Models.ResponseModels.Session;
using Profile = BlazorApp.Domain.Entities.Identity.Profile;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Models.ResponseModels.User;
using BlazorApp.Shared.Models.RequestModels.User;
using BlazorApp.Shared.Models.ResponseModel.Session;
using BlazorApp.Shared.Models.ResponseModel.User;
using BlazorApp.Shared.Models.ResponseModel.Quiz;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.Shared.Models.RequestModels.Quiz;
using BlazorApp.Models.RequestModels.Quiz;

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
                .ForMember(t => t.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToISO()));

            CreateMap<Quiz, SmallQuizResponseModel>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(t => t.QuestionsCount, opt => opt.Ignore())
                .ForMember(t => t.Topic, opt => opt.MapFrom(src => src.Topic.Name))
                .ForMember(t => t.Author, opt => opt.MapFrom(src => src.Author));

            CreateMap<QuizQuestion, QuizQuestionResponseModel>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(t => t.QuizId, opt => opt.MapFrom(src => src.QuizId))
                .ForMember(t => t.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToISO()));

            CreateMap<QuizAnswer, QuizAnswerResponseModel>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(t => t.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(t => t.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToISO()));




            #endregion
            #region ClientModels To ServerModels
            #region Requests
            #endregion
            #region Responses
            #endregion
            CreateMap<RegisterRequestModel, RegistrationRequest>()
                .ForMember(t => t.Email, opt => opt.MapFrom(x => x.Email))
                .ForMember(t => t.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(t => t.ConfirmPassword, opt => opt.MapFrom(src => src.ConfirmPassword));
            CreateMap<RegisterResponseModel, RegistrationResponse>()
               .ForMember(t => t.Email, opt => opt.MapFrom(x => x.Email));

            CreateMap<UserResponseModel, UserResponse>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.LastName))
                .ForMember(x => x.IsBlocked, opt => opt.MapFrom(x => x.IsBlocked));

            CreateMap<UserRoleResponseModel, UserRoleResponse>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.LastName))
                .ForMember(x => x.Role, opt => opt.MapFrom(x => x.Role))
                .ForMember(x => x.IsBlocked, opt => opt.MapFrom(x => x.IsBlocked));

            CreateMap<QuizResponseModel, QuizResponse>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Author, opt => opt.MapFrom(x => x.Author))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(x => x.IsActive))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
                .ForMember(x => x.TimeToPass, opt => opt.MapFrom(x => x.TimeToPass))
                .ForMember(x => x.Questions, opt => opt.MapFrom(x => x.Questions))
                .ForMember(x => x.UsersJoined, opt => opt.MapFrom(x => x.UsersJoined))
                .ForMember(x => x.Topic, opt => opt.MapFrom(x => x.Topic));

            CreateMap<QuizQuestionResponseModel, QuizQuestionResponse>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Question, opt => opt.MapFrom(x => x.Question))
                .ForMember(x => x.QuizId, opt => opt.MapFrom(x => x.QuizId))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
                .ForMember(x => x.Answers, opt => opt.MapFrom(x => x.Answers));

            CreateMap<QuizAnswerResponseModel, QuizAnswerResponse>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Text, opt => opt.MapFrom(x => x.Text))
                .ForMember(x => x.QuestionId, opt => opt.MapFrom(x => x.QuestionId))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(x => x.CreatedAt))
                .ForMember(x => x.IsCorrect, opt => opt.MapFrom(x => x.IsCorrect));

            CreateMap<QuizRequest, QuizRequestModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.Author, opt => opt.MapFrom(x => x.Author))
                .ForMember(x => x.Topic, opt => opt.MapFrom(x => x.Topic))
                .ForMember(x => x.TimeToPass, opt => opt.MapFrom(x => x.TimeToPass));

            CreateMap<QuizQuestionRequest, QuizQuestionRequestModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.QuizId, opt => opt.MapFrom(x => x.QuizId))
                .ForMember(x => x.Question, opt => opt.MapFrom(x => x.Question));

            CreateMap<QuizAnswerRequest, QuizAnswerRequestModel>()
                .ForMember(x => x.QuestionId, opt => opt.MapFrom(x => x.QuestionId))
                .ForMember(x => x.Text, opt => opt.MapFrom(x => x.Text))
                .ForMember(x => x.IsCorrect, opt => opt.MapFrom(x => x.IsCorrect));

            CreateMap<SmallQuizResponseModel, SmallQuizResponse>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(t => t.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(t => t.QuestionsCount, opt => opt.MapFrom(src => src.QuestionsCount))
                .ForMember(t => t.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<TopicResponseModel, TopicResponse>()
                .ForMember(t => t.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<ProfileRequest, UserProfileRequestModel>()
                .ForMember(t => t.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(t => t.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(t => t.ImageId, opt => opt.MapFrom(src => src.ImageId));

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
            CreateMap<SmallUserResultResponseModel, SmallUserResultResponse>()
                .ForMember(t => t.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(t => t.QuizId, opt => opt.MapFrom(x => x.QuizId))
                .ForMember(t => t.QuizName, opt => opt.MapFrom(x => x.QuizName))
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
            CreateMap<UserResultResponseModel, UserResultResponse>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.UserId, opt => opt.MapFrom(x => x.UserId))
                .ForMember(x => x.Quiz, opt => opt.MapFrom(x => x.Quiz))
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.UserName))
                .ForMember(x => x.ResultMark, opt => opt.MapFrom(x => x.ResultMark))
                .ForMember(x => x.CountOfCurrentAnswers, opt => opt.MapFrom(x => x.CountOfCurrentAnswers))
                .ForMember(x => x.CountOfIncorrectAnswers, opt => opt.MapFrom(x => x.CountOfIncorrectAnswers))
                .ForMember(x => x.JoinedAt, opt => opt.MapFrom(x => x.JoinedAt));
            #endregion
            CreateMap<UserDevice, UserDeviceResponseModel>()
                .ForMember(t => t.AddedAt, opt => opt.MapFrom(src => src.AddedAt.ToISO()));

            #region User model

            CreateMap<UserProfileRequestModel, Profile>()
                .ForMember(t => t.Id, opt => opt.Ignore())
                .ForMember(t => t.User, opt => opt.Ignore());

            CreateMap<Profile, UserResponseModel>()
                .ForMember(t => t.Email, opt => opt.MapFrom(x => x.User != null ? x.User.Email : ""))
                .ForMember(t => t.PhoneNumber, opt => opt.MapFrom(x => x.User != null ? x.User.PhoneNumber : ""))
                .ForMember(t => t.IsBlocked, opt => opt.MapFrom(x => x.User != null ? !x.User.IsActive : false));

            CreateMap<ApplicationUser, UserBaseResponseModel>()
               .IncludeAllDerived();

            CreateMap<ApplicationUser, UserResponseModel>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.Profile.FirstName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.Profile.LastName))
                .ForMember(x => x.IsBlocked, opt => opt.MapFrom(x => !x.IsActive))
                .IncludeAllDerived();

            CreateMap<ApplicationUser, UserRoleResponseModel>();

            #endregion

        }
    }
}
