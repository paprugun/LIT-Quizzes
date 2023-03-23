using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels.Session;
using BlazorApp.Shared.Models.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Refresh tokens
        /// </summary>
        /// <param name="refreshToken">Refresh token</param>
        /// <param name="roles">Roles</param>
        /// <returns></returns>
        Task<TokenResponseModel> RefreshTokenAsync(string refreshToken, List<string> roles);

        /// <summary>
        /// Register a new user using email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<RegisterResponseModel> Register(RegisterRequestModel model);

        /// <summary>
        /// Register a new admin using email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<RegisterResponseModel> RegisterAdmin(RegisterRequestModel model);
        /// <summary>
        /// Login using email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<LoginResponseModel> Login(LoginRequestModel model);

        /// <summary>
        /// Admin login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<LoginResponseModel> AdminLogin(AdminLoginRequestModel model);

        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        Task Logout();
    }
}
