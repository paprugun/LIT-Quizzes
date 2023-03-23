using AutoMapper;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Models.RequestModels.CursorPagination;
using BlazorApp.Models.ResponseModels.Catalog;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Services.Interfaces.PageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Services.PageServices
{
    public class CatalogPageService : ICatalogPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CatalogPageService(IUnitOfWork unitOfWork,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CatalogPaginationResponseModel> GetCatalog(CatalogPaginationRequestModel model)
        {
            var quizzes = _unitOfWork.Repository<Quiz>().Get(x => x.Id >= model.LastId
                                                    && x.IsActive)
                                                    .Select(x => new SmallQuizResponseModel
                                                    {
                                                        Id = x.Id,
                                                        Name = x.Name,
                                                        QuestionsCount = _unitOfWork.Repository<QuizQuestion>().Get(w => w.QuizId == x.Id).Count(),
                                                        Author = x.Author,
                                                        Topic = x.Topic.Name,
                                                    });

            var response = new CatalogPaginationResponseModel();
            response.Quizzes = quizzes.OrderBy(x => x.Name).Take(9).ToList();
            return response;
        }
    }
}
