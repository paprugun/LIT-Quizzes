using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Models.ResponseModels.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces.PageServices
{
    public interface IAdminPageService
    {
        Task<IEnumerable<SmallQuizResponseModel>> GetTable();
        Task<UsersStatsResponseModel> GetUsersStats();
        Task<QuizzesStatsResponseModel> GetQuizzesStats();
        Task<TopicsStatsResponseModel> GetTopicsStats();
    }
}
