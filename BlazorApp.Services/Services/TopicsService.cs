using AutoMapper;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared.Models.RequestModels.Quiz;
using BlazorApp.Shared.Models.ResponseModel.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Services
{
    public class TopicsService : ITopicsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TopicsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TopicResponseModel> CreateTopic(string name)
        {
            var topic = new Topic() {Name = name};
            _unitOfWork.Repository<Topic>().Insert(topic);
            _unitOfWork.SaveChanges();
            var response = _mapper.Map<TopicResponseModel>(topic);
            return response;
        }

        public async Task<string> DeleteTopic(int id)
        {
            _unitOfWork.Repository<Topic>().DeleteById(id);
            _unitOfWork.SaveChanges();
            return $"{id} topic was deleted";
        }

        public async Task<TopicResponseModel> EditTopic(int id, string name)
        {
            var topic = _unitOfWork.Repository<Topic>().Get(x => x.Id == id).FirstOrDefault();
            topic.Name = name;
            _unitOfWork.Repository<Topic>().Update(topic);
            _unitOfWork.SaveChanges();
            var response = _mapper.Map<TopicResponseModel>(topic);
            return response;
        }

        public async Task<IEnumerable<TopicResponseModel>> GetAllTopics()
        {
            var topics = _unitOfWork.Repository<Topic>().GetAll();
            var response = _mapper.Map<IEnumerable<TopicResponseModel>>(topics);
            return response;
        }

        public async Task<TopicResponseModel> GetTopicById(int id)
        {
            var topic = _unitOfWork.Repository<Topic>().Get(x => x.Id == id).FirstOrDefault();
            var response = _mapper.Map<TopicResponseModel>(topic);
            return response;
        }
    }
}
