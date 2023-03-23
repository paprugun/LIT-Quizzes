using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Shared.Models.RequestModels.Quiz;
using BlazorApp.Shared.Models.ResponseModel.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces
{
    public interface ITopicsService
    {
        Task<IEnumerable<TopicResponseModel>> GetAllTopics();
        Task<TopicResponseModel> GetTopicById(int id);
        Task<string> DeleteTopic(int id);
        Task<TopicResponseModel> CreateTopic(string name);
        Task<TopicResponseModel> EditTopic(int id, string name);
    }
}
