using BlazorApp.Shared.Models.Enums;
using BlazorApp.Shared.Models.RequestModels.Pagination;
using BlazorApp.Shared.Models.ResponseModel.Pagination;
using BlazorApp.Shared.Models.ResponseModel.Pagination.CursorPagination;
using BlazorApp.Shared.Models.ResponseModel.Quiz;
using BlazorApp.Shared.Models.ResponseModel.Session;
using BlazorApp.Shared.Models.ResponseModels.Quiz;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BlazorApp.Client.Services
{
    public interface IQuizService 
    {
        Task<JsonPaginationResponse<List<SmallQuizResponse>>> GetAll(PaginationRequestModel<QuizTableColumn> model);
        Task<JsonPaginationResponse<List<SmallCourseResponseModel>>> GetAllCourses(PaginationRequestModel<CourseTableColumn> model);

        Task<JsonPaginationResponse<List<SmallCourseResponseModel>>> GetAllAssignments(PaginationRequestModel<CourseTableColumn> model);
    }

    public class QuizService : IQuizService
    {
        private readonly HttpClient _httpClient;

        public QuizService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JsonPaginationResponse<List<SmallQuizResponse>>> GetAll(PaginationRequestModel<QuizTableColumn> model)
        {
            using var response = await _httpClient.PostAsJsonAsync($"api/v1/quiz", model);
            var quizzes = JsonConvert.DeserializeObject<JsonPaginationResponse<List<SmallQuizResponse>>>(await response.Content.ReadAsStringAsync());
			return quizzes;
        }

        public async Task<JsonPaginationResponse<List<SmallCourseResponseModel>>> GetAllCourses(PaginationRequestModel<CourseTableColumn> model)
        {
            using var response = await _httpClient.PostAsJsonAsync($"api/v1/courses", model);
            var quizzes = JsonConvert.DeserializeObject<JsonPaginationResponse<List<SmallCourseResponseModel>>>(await response.Content.ReadAsStringAsync());
            return quizzes;
        }

        public async Task<JsonPaginationResponse<List<SmallCourseResponseModel>>> GetAllAssignments(PaginationRequestModel<CourseTableColumn> model)
        {
            using var response = await _httpClient.PostAsJsonAsync($"api/v1/courses/assignments", model);
            var quizzes = JsonConvert.DeserializeObject<JsonPaginationResponse<List<SmallCourseResponseModel>>>(await response.Content.ReadAsStringAsync());
            return quizzes;
        }
    }
}
