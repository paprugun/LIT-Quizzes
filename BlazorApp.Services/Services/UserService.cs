using BlazorApp.Common.Exceptions;
using BlazorApp.Common.Extensions;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.RequestModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Base.CursorPagination;
using BlazorApp.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlazorApp.Models.ResponseModels.Session;
using Microsoft.AspNetCore.Identity;

namespace BlazorApp.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = null;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper = null;
        private readonly IJWTService _jwtService;
        private readonly UserManager<ApplicationUser> _userManager = null;

        private bool _isUserSuperAdmin = false;
        private bool _isUserAdmin = false;
        private int? _userId = null;

        public UserService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider,
            IJWTService jwtService,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _jwtService = jwtService;
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

        public PaginationResponseModel<UserTableRowResponseModel> GetAll(PaginationRequestModel<UserTableColumn> model, bool getAdmins = false)
        {
            List<UserTableRowResponseModel> response = new List<UserTableRowResponseModel>();

            var search = !string.IsNullOrEmpty(model.Search) && model.Search.Length > 1;

            //!x.UserRoles.Any(w => (_userIsSuperAdmin && w.Role.Name != Role.Admin) && w.Role.Name != Role.SuperAdmin)

            var users = _unitOfWork.Repository<ApplicationUser>().Get(x => !x.IsDeleted
                                            && (!search || (x.Email.Contains(model.Search) || x.Profile.FirstName.Contains(model.Search) || x.Profile.LastName.Contains(model.Search)))
                                            && (getAdmins
                                                    ? (x.UserRoles.Any(w => w.Role.Name == Role.Admin) || x.UserRoles.Any(w => w.Role.Name == Role.SuperAdmin))
                                                    : x.UserRoles.Any(w => w.Role.Name == Role.User))
                                            //&& (_isUserSuperAdmin || !x.UserRoles.Any(w => w.Role.Name == Role.Admin))
                                            )
                                        .TagWith(nameof(GetAll) + "_GetUsers")
                                        .Include(w => w.UserRoles)
                                            .ThenInclude(w => w.Role)
                                        .Select(x => new
                                        {
                                            Email = x.Email,
                                            FirstName = x.Profile.FirstName,
                                            LastName = x.Profile.LastName,
                                            IsBlocked = !x.IsActive,
                                            RegisteredAt = x.RegistratedAt,
                                            Id = x.Id
                                        });



            if (search)
                users = users.Where(x => x.Email.Contains(model.Search) || x.FirstName.Contains(model.Search) || x.LastName.Contains(model.Search));

            int count = users.Count();

            if (model.Order != null)
                users = users.OrderBy(model.Order.Key.ToString(), model.Order.Direction == SortingDirection.Asc);

            users = users.Skip(model.Offset).Take(model.Limit);

            response = users.Select(x => new UserTableRowResponseModel
            {
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                IsBlocked = x.IsBlocked,
                RegisteredAt = x.RegisteredAt.ToISO(),
                Id = x.Id

            }).ToList();

            return new(response, count);
        }

        public CursorPaginationBaseResponseModel<UserTableRowResponseModel> GetAll(CursorPaginationRequestModel<UserTableColumn> model, bool getAdmins = false)
        {
            var search = !string.IsNullOrEmpty(model.Search) && model.Search.Length > 1;

            var users = _unitOfWork.Repository<ApplicationUser>().Get(x => !x.IsDeleted
                                            && !x.UserRoles.Any(w => w.Role.Name == Role.SuperAdmin)
                                            && (!search || (x.Email.Contains(model.Search) || x.Profile.FirstName.Contains(model.Search) || x.Profile.LastName.Contains(model.Search)))
                                            && (getAdmins ? x.UserRoles.Any(w => w.Role.Name == Role.Admin) : x.UserRoles.Any(w => w.Role.Name == Role.User))
                                            && (_isUserSuperAdmin || !x.UserRoles.Any(w => (w.Role.Name == Role.Admin))))
                                        .TagWith(nameof(GetAll) + "_GetUsers")
                                        .Select(x => new
                                        {
                                            Email = x.Email,
                                            FirstName = x.Profile.FirstName,
                                            LastName = x.Profile.LastName,
                                            IsBlocked = !x.IsActive,
                                            RegisteredAt = x.RegistratedAt,
                                            Id = x.Id
                                        });

            if (model.Order != null)
                users = users.OrderBy(model.Order.Key.ToString(), model.Order.Direction == SortingDirection.Asc);

            var userList = users.ToList();

            var offset = 0;

            if (model.LastId.HasValue)
            {
                var item = userList.FirstOrDefault(u => u.Id == model.LastId);

                if (item is null)
                    throw new CustomException(HttpStatusCode.BadRequest, "lastId", "There is no user with specific id in selection");

                offset = userList.IndexOf(item) + 1;
            }

            users = users.Skip(offset).Take(model.Limit + 1);

            var response = users.Select(x => new UserTableRowResponseModel
            {
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                IsBlocked = x.IsBlocked,
                RegisteredAt = x.RegisteredAt.ToISO(),
                Id = x.Id
            });

            int? nextCursorId = null;

            if (users.Count() > model.Limit)
            {
                response = response.Take(model.Limit);
                nextCursorId = response.AsEnumerable().LastOrDefault()?.Id;
            }

            return new(response.ToList(), nextCursorId);
        }

        public UserResponseModel SoftDeleteUser(int id)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(w => w.Id == id && !w.UserRoles.Any(x => x.Role.Name == Role.SuperAdmin) 
                                                                && (!w.UserRoles.Any(x => x.Role.Name == Role.Admin) || _isUserSuperAdmin))
                                                              .TagWith(nameof(SoftDeleteUser) + "_GetUser")
                                                              .Include(w => w.Profile)
                                                              .Include(w => w.QuizzesResults)
                                                              .ThenInclude(w => w.Quiz)
                                                              .FirstOrDefault();

            if (user == null)
                throw new CustomException(HttpStatusCode.BadRequest, "userId", "User is not found");

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<ApplicationUser>().Update(user);
            _unitOfWork.SaveChanges();

            return _mapper.Map<UserRoleResponseModel>(user);
        }

        public UserResponseModel DisbanUser(int id)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(w => w.Id == id && !w.UserRoles.Any(x => x.Role.Name == Role.SuperAdmin)
                                                                && (!w.UserRoles.Any(x => x.Role.Name == Role.Admin) || _isUserSuperAdmin))
                                                              .TagWith(nameof(SoftDeleteUser) + "_GetUser")
                                                              .Include(w => w.Profile)
                                                              .Include(w => w.QuizzesResults)
                                                              .ThenInclude(w => w.Quiz)
                                                              .FirstOrDefault();

            if (user == null)
                throw new CustomException(HttpStatusCode.BadRequest, "userId", "User is not found");

            user.IsDeleted = false;
            user.DeletedAt = DateTime.UtcNow;

            _unitOfWork.Repository<ApplicationUser>().Update(user);
            _unitOfWork.SaveChanges();

            return _mapper.Map<UserRoleResponseModel>(user);
        }

        public async Task HardDeleteUser(int id)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(w => w.Id == id)
                                      .TagWith(nameof(SoftDeleteUser) + "_GetUser")
                                      .Include(w => w.Profile)
                                      .FirstOrDefault();

            if (user == null)
                throw new CustomException(HttpStatusCode.BadRequest, "userId", "User is not found");

            _unitOfWork.Repository<ApplicationUser>().Delete(user);
            _unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<UserRoleResponseModel>> GetUsers()
        {
            var response = new List<UserRoleResponseModel>();

            var users = _unitOfWork.Repository<ApplicationUser>().Get(w => !w.UserRoles.Any(x => x.Role.Name == Role.Admin) && w.Id != _userId)
                                                                .TagWith(nameof(GetUsers) + "_GetUsers")
                                                                .Include(w => w.Profile)
                                                                .Include(w => w.UserRoles)
                                                                    .ThenInclude(w => w.Role)
                                                                .ToList();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                response.Add(_mapper.Map<ApplicationUser, UserRoleResponseModel>(user, opt => opt.AfterMap((src, dest) =>
                {
                    dest.Role = (roles != null) ? roles.SingleOrDefault() : "none";
                })));
            }
            return response;
        }

        public async Task<IEnumerable<UserRoleResponseModel>> GetAll()
        {
            var response = new List<UserRoleResponseModel>();

            var users = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id != _userId)
                                                                 .TagWith(nameof(GetAll) + "_GetUsers")
                                                                 .Include(x => x.Profile)
                                                                 .Include(x => x.UserRoles)
                                                                    .ThenInclude(x => x.Role)
                                                                 .ToList();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                response.Add(_mapper.Map<ApplicationUser, UserRoleResponseModel>(user, opt => opt.AfterMap((src, dest) =>
                {
                    dest.Role = (roles != null) ? roles.SingleOrDefault() : "none";
                })));
            }

            return response;
        }

        public async Task<UserRoleResponseModel> GetUserInfo(int id)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == id)
                                                                .TagWith(nameof(GetUserInfo) + "_GetUser")
                                                                .Include(w => w.Profile)
                                                                .FirstOrDefault();

            if(user == null)
                throw new CustomException(HttpStatusCode.BadRequest, "userId", "User is not found");

            var roles = await _userManager.GetRolesAsync(user);

            var response = _mapper.Map<ApplicationUser, UserRoleResponseModel>(user, opt => opt.AfterMap((src, dest) =>
            {
                dest.Role = (roles != null) ? roles.SingleOrDefault() : "none";
            })); 

            return response;
        }


        public async Task SetRole(string role, int id)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == id)
                                                                    .TagWith(nameof(SetRole) + "_GetUser")
                                                                    .Include(w => w.UserRoles)
                                                                        .ThenInclude(w => w.Role)
                                                                    .FirstOrDefault();
            if (user == null)
                throw new CustomException(HttpStatusCode.BadRequest, "invalid userId", "invalid userId");

            var existingRole = _unitOfWork.Repository<ApplicationRole>().Get(x => x.Name == role).FirstOrDefault();

            if (existingRole == null)
            {
                existingRole = new ApplicationRole() { Name = role };
                _unitOfWork.Repository<ApplicationRole>().Insert(existingRole);
                _unitOfWork.SaveChanges();
            }

            if (role == Role.User && user.UserRoles.Any(x => x.Role.Name == Role.Admin))
                user.UserRoles.Remove(user.UserRoles.FirstOrDefault(x => x.Role.Name == Role.Admin));

            if (role == Role.Admin && user.UserRoles.Any(x => x.Role.Name == Role.User))
                user.UserRoles.Remove(user.UserRoles.FirstOrDefault(x => x.Role.Name == Role.User));

            if (!user.UserRoles.Any(x => x.Role.Name == role))
                user.UserRoles.Add(new ApplicationUserRole() { Role = existingRole });

            _unitOfWork.Repository<ApplicationUser>().Update(user);
            _unitOfWork.SaveChanges();
        }

        public async Task DeleteRole(string role, int id)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().Get(x => x.Id == id && x.UserRoles.Any(c => c.Role.Name == role))
                                                                .Include(w => w.UserRoles)
                                                                    .ThenInclude(w => w.Role)
                                                                .FirstOrDefault();
            
            if (user != null)
                user.UserRoles.Remove(user.UserRoles.FirstOrDefault(x => x.Role.Name == role));
            if (user.UserRoles.Count == 0)
                user.UserRoles.Add(new ApplicationUserRole() { Role = new ApplicationRole() { Name = Role.User } });

            _unitOfWork.Repository<ApplicationUser>().Update(user);
            _unitOfWork.SaveChanges();
        }
    }
}
