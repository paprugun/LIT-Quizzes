using AutoMapper;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Models.ResponseModels.Stats;
using BlazorApp.Services.Interfaces.PageServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Services.PageServices
{
    public class AdminPageService : IAdminPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminPageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<QuizzesStatsResponseModel> GetQuizzesStats()
        {
            var response = new QuizzesStatsResponseModel();
            response.QuizzesCount = _unitOfWork.Repository<Quiz>().GetAll().Count();
            response.QuestionsCount = _unitOfWork.Repository<QuizQuestion>().GetAll().Count();
            response.AnswersCount = _unitOfWork.Repository<QuizAnswer>().GetAll().Count();
            return response;
        }

        public async Task<IEnumerable<SmallQuizResponseModel>> GetTable()
        {
            var quizzes = _unitOfWork.Repository<Quiz>().GetAll().Take(10);
            var response = _mapper.Map<IEnumerable<SmallQuizResponseModel>>(quizzes);
            return response;
        }

        public async Task<TopicsStatsResponseModel> GetTopicsStats()
        {
            var response = new TopicsStatsResponseModel();
            response.TopicCount = _unitOfWork.Repository<Topic>().GetAll().Count();
            response.CSharpQuizzesCount = _unitOfWork.Repository<Quiz>().Get(x => x.Topic.Name == "C#")
                                                                        .Include(w => w.Topic)
                                                                        .Count();

            response.JSQuizzesCount = _unitOfWork.Repository<Quiz>().Get(x => x.Topic.Name == "JavaScript")
                                                                        .Include(w => w.Topic)
                                                                        .Count();

            response.CPPQuizesCount = _unitOfWork.Repository<Quiz>().Get(x => x.Topic.Name == "C++")
                                                                        .Include(w => w.Topic)
                                                                        .Count();
            return response;
        }

        public async Task<UsersStatsResponseModel> GetUsersStats()
        {
            var response = new UsersStatsResponseModel();
            response.RegisteredCount = _unitOfWork.Repository<ApplicationUser>().GetAll().Count();
            response.BlockedCount = _unitOfWork.Repository<ApplicationUser>().Get(x => x.IsDeleted).Count();
            response.QuizzesPassedCount = _unitOfWork.Repository<UsersResults>().GetAll().Count();
            return response;
        }
    }
}
