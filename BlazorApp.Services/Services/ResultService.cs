using AutoMapper;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Services
{
    public class ResultService : IResultsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IQuizAdminService _adminService;

        public ResultService(IUnitOfWork unitOfWork, IMapper mapper, IQuizAdminService adminService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _adminService = adminService;
        }

        public async Task DeleteResult(int resultId)
        {
            _unitOfWork.Repository<UsersResults>().DeleteById(resultId);
            _unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<SmallUserResultResponseModel>> GetAllResults(int id)
        {
            var results = _unitOfWork.Repository<UsersResults>().Get(x => x.QuizId == id)
                                                                .Include(w => w.User)
                                                                    .ThenInclude(w => w.Profile)
                                                                .AsQueryable();
            var response = _mapper.Map<IEnumerable<SmallUserResultResponseModel>>(results);
            return response;
        }

        public async Task<UserResultResponseModel> GetResult(int id)
        {
            var result = _unitOfWork.Repository<UsersResults>().Get(x => x.Id == id)
                                                               .Include(w => w.Quiz)
                                                               .Include(w => w.User)
                                                                   .ThenInclude(w => w.Profile)
                                                               .FirstOrDefault();
            var response = _mapper.Map<UserResultResponseModel>(result);
            response.Quiz = await _adminService.GetQuizById(response.Quiz.Id);
            response.UserName = result.User.Profile.FullName;
            return response;
        }
    }
}
