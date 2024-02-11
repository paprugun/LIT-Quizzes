using AutoMapper;
using BlazorApp.Common.Exceptions;
using BlazorApp.Common.Extensions;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Domain.Entities.QuizEntities;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.Models.ResponseModels.Session;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager = null;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = null;

        private bool _isUserSuperAdmin = false;
        private bool _isUserAdmin = false;
        private int? _userId = null;

        public ProfileService(IUnitOfWork unitOfWork, 
                                IMapper mapper, 
                                IHttpContextAccessor httpContextAccessor, 
                                UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

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

        public async Task<UserRoleResponseModel> Edit(UserProfileRequestModel model)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == _userId)
                            .Include(w => w.UserRoles)
                                .ThenInclude(w => w.Role)
                            .Include(w => w.Profile)
                            .FirstOrDefault(); 

            user.Profile.FirstName = model.FirstName;
            user.Profile.LastName = model.LastName;
            user.Profile.AvatarId = model.ImageId;
            
            _unitOfWork.Repository<ApplicationUser>().Update(user);
            _unitOfWork.SaveChanges();

            var roles = await _userManager.GetRolesAsync(user);

            var response = _mapper.Map<ApplicationUser, UserRoleResponseModel>(user, opt => opt.AfterMap((src, dest) =>
            {
                dest.Role = (roles != null) ? roles.SingleOrDefault() : "none";
            }));
            return response;
        }

        public async Task DeleteAccount()
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(w => w.Id == _userId && !w.UserRoles.Any(x => x.Role.Name == Role.SuperAdmin) && ((!w.UserRoles.Any(x => x.Role.Name == Role.Admin) || _isUserSuperAdmin)))
                                                  .Include(w => w.Profile)
                                                  .Include(w => w.QuizzesResults)
                                                  .FirstOrDefault();

            if (user == null)
                throw new CustomException(HttpStatusCode.BadRequest, "userId", "User is not found");

            _unitOfWork.Repository<ApplicationUser>().Delete(user);
            _unitOfWork.SaveChanges();
        }

		public async Task DeleteResult(int resultId)
		{
            _unitOfWork.Repository<UsersResults>().DeleteById(resultId);
            _unitOfWork.SaveChanges();
		}

		public async Task<IEnumerable<SmallUserResultResponseModel>> GetAllResults()
        {   var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == _userId)
                .Include(w => w.QuizzesResults)
                    .ThenInclude(w => w.Quiz)
                    .FirstOrDefault();         
            var response = _mapper.Map<IEnumerable<SmallUserResultResponseModel>>(user.QuizzesResults);
            return response;
        }

        public async Task<UserResultResponseModel> GetResult(int resultId)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == _userId)
                .Include(w => w.QuizzesResults)
                    .ThenInclude(w => w.Quiz)
                        .ThenInclude(w => w.Questions)
                            .ThenInclude(w => w.Answers)
                .FirstOrDefault();

            if (user.QuizzesResults.FirstOrDefault(w => w.Id == resultId) == null)
                throw new CustomException(HttpStatusCode.BadRequest, "resultId is invalid", "result isnt found");

            var response = _mapper.Map<UserResultResponseModel>(user.QuizzesResults.FirstOrDefault(x => x.Id == resultId));
            return response;
        }
    }
}
