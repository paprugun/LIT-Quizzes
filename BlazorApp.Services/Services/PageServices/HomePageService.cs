using AutoMapper;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Models.ResponseModels.MainPage;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Services.Interfaces.PageServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Services.PageServices
{
    public class HomePageService : IHomePageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Random _rnd;
        public HomePageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _rnd = new Random();
        }

        public async Task<CPPCardInfo> CPPCardInfo()
        {
            var response = new CPPCardInfo();
            response.CountOfPassed = _unitOfWork.Repository<UsersResults>().Get(x => x.Quiz.Topic.Name == "C++")
                                                                            .Include(w => w.Quiz)
                                                                                .ThenInclude(w => w.Topic)
                                                                            .Count();

            response.CountOfQuizzez = _unitOfWork.Repository<Quiz>().Get(x => x.Topic.Name == "C++")
                                                                    .Include(w => w.Topic)
                                                                    .Count();

            var quizzes = _unitOfWork.Repository<Quiz>().Get(x => x.Topic.Name == "C++")
                                                        .Include(w => w.Topic).ToList();
            var questions = new List<QuizQuestion>();

            foreach (var item in quizzes)
            {
                var questionstemp = _unitOfWork.Repository<QuizQuestion>().Get(x => x.QuizId == item.Id);
                questions.AddRange(questionstemp);
            }
            response.CountOfQuestions = questions.Count;

            return response;
        }

        public async Task<CSharpCardInfo> CSCardInfo()
        {
            var response = new CSharpCardInfo();
            response.CountOfPassed = _unitOfWork.Repository<UsersResults>().Get(x => x.Quiz.Topic.Name == "C#")
                                                                            .Include(w => w.Quiz)
                                                                                .ThenInclude(w => w.Topic)
                                                                            .Count();

            response.CountOfQuizzez = _unitOfWork.Repository<Quiz>().Get(x => x.Topic.Name == "C#")
                                                                    .Include(w => w.Topic)
                                                                    .Count();

            var quizzes = _unitOfWork.Repository<Quiz>().Get(x => x.Topic.Name == "C#")
                                                        .Include(w => w.Topic)
                                                        .ToList();
            var questions = new List<QuizQuestion>();

            foreach (var item in quizzes) 
            {
                var questionstemp = _unitOfWork.Repository<QuizQuestion>().Get(x => x.QuizId == item.Id);
                questions.AddRange(questionstemp);
            }

            response.CountOfQuestions = questions.Count;

            return response;
        }

        public async Task<IEnumerable<SmallQuizResponseModel>> GetFeaturesQuizzes()
        {
            var quizzes = _unitOfWork.Repository<Quiz>().Get(w => w.IsActive == true).Include(w => w.Topic).ToList();
            var response = new List<SmallQuizResponseModel>();

            while (response.Count != 3)
            {
                var index = _rnd.Next(0, quizzes.Count - 1);
                var mappedQuiz = _mapper.Map<SmallQuizResponseModel>(quizzes[index]);
                var quiz = response.FirstOrDefault(x => x.Id == mappedQuiz.Id);
                if (quiz == null)
                {
                    var questionsCount = _unitOfWork.Repository<QuizQuestion>().Get(x => x.QuizId == quizzes[index].Id).Count();
                    mappedQuiz.QuestionsCount = questionsCount;
                    response.Add(mappedQuiz);
                }
            }
            return _mapper.Map<IEnumerable<SmallQuizResponseModel>>(response);

        }

        public async Task<JSCardInfo> JSCardInfo()
        {
            var response = new JSCardInfo();
            response.CountOfPassed = _unitOfWork.Repository<UsersResults>().Get(x => x.Quiz.Topic.Name == "JavaScript")
                                                                            .Include(w => w.Quiz)
                                                                                .ThenInclude(w => w.Topic)
                                                                            .Count();

            response.CountOfQuizzez = _unitOfWork.Repository<Quiz>().Get(x => x.Topic.Name == "JavaScript")
                                                                    .Include(w => w.Topic)
                                                                    .Count();
            var quizzes = _unitOfWork.Repository<Quiz>().Get(x => x.Topic.Name == "JavaScript")
                                                        .Include(w => w.Topic).ToList();
            var questions = new List<QuizQuestion>();

            foreach (var item in quizzes)
            {
                var questionstemp = _unitOfWork.Repository<QuizQuestion>().Get(x => x.QuizId == item.Id);
                questions.AddRange(questionstemp);
            }
            response.CountOfQuestions = questions.Count;

            return response;
        }
    }
}
