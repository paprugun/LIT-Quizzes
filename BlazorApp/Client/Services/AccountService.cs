using BlazorApp.Client.Pages.Admin;
using BlazorApp.Shared.Models.RequestModels.User;
using BlazorApp.Shared.Models.ResponseModel.Pagination;
using BlazorApp.Shared.Models.ResponseModel.Session;
using BlazorApp.Shared.Models.ResponseModel.User;
using Blazorise;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BlazorApp.Client.Services
{
    public interface IAccountService
    {
        Task<LoginResponseModel> Login(LoginRequest model);
        Task Logout();
        Task<LoginResponseModel> AdminLogin(AdminLoginRequest model);

        Task<RegistrationResponse> Register(RegistrationRequest model);
        Task<LoginResponseModel> RefreshToken();
    }
    public class AccountService : IAccountService
    {
        private HttpClient _httpClient;
        public AccountService(HttpClient client)
        {
            _httpClient = client;
        }
        public async Task<LoginResponseModel> AdminLogin(AdminLoginRequest model)
        {
            using var response = await _httpClient.PostAsJsonAsync("api/v1/admin-sessions", model);
			var _model = JsonConvert.DeserializeObject<JsonResponse<LoginResponseModel>>(await response.Content.ReadAsStringAsync());
			return _model.Data;
        }

        public async Task<LoginResponseModel> Login(LoginRequest model)
        {
            using var response = await _httpClient.PostAsJsonAsync("api/v1/sessions", model);
			var _model = JsonConvert.DeserializeObject<JsonResponse<LoginResponseModel>>(await response.Content.ReadAsStringAsync());
			return _model.Data;
        }

        public async Task Logout()
        {
            await _httpClient.DeleteAsync("api/v1/sessions");
        }

        public async Task AdminLogout()
        {
            await _httpClient.DeleteAsync("api/v1/admin-sessions");
        }

        public async Task<LoginResponseModel> RefreshToken()
        {
            throw new System.NotImplementedException();
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest model)
        {
            using var response = await _httpClient.PostAsJsonAsync("api/v1/users", model);
			var _model = JsonConvert.DeserializeObject<JsonResponse<RegistrationResponse>>(await response.Content.ReadAsStringAsync());
			return _model.Data;
        }
    }
}
