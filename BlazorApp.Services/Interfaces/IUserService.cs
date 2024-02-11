using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.RequestModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels.Session;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="model"></param>
        /// <param name="getAdmins"></param>
        /// <returns></returns>
        PaginationResponseModel<UserTableRowResponseModel> GetAll(PaginationRequestModel<UserTableColumn> model, bool getAdmins = false);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="model"></param>
        /// <param name="getAdmins"></param>
        /// <returns></returns>
        CursorPaginationBaseResponseModel<UserTableRowResponseModel> GetAll(CursorPaginationRequestModel<UserTableColumn> model, bool getAdmins = false);

        /// <summary>
        /// Get user info
        /// </summary>
        /// <returns></returns>
        Task<UserRoleResponseModel> GetUserInfo(int id);

        /// <summary>
        /// Get user info
        /// </summary>
        /// <returns></returns>
        Task SetRole(string role, int id);

        /// <summary>
        /// Get user info
        /// </summary>
        /// <returns></returns>
        Task DeleteRole(string role, int id);

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserRoleResponseModel>> GetAll();

        /// <summary>
        /// Soft delete user (leave in db)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserResponseModel SoftDeleteUser(int id);

        UserResponseModel DisbanUser(int id);

        /// <summary>
        /// Hard delete user (delete from db)
        /// </summary>
        /// <param name="id"></param>
        Task HardDeleteUser(int id);

    }
}
