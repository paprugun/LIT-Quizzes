using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.RequestModels.Course;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Course;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces.CourseServices
{
    public interface ICourseService
    {
        Task<CourseResponseModel> CreateCourse(CourseRequestModel model);

        Task<AdminCourseResponseModel> UpdateCourse(CourseRequestModel model, int id);

        Task DeleteCourse(int id);

        Task<CourseResponseModel> GetCourse(int id);

        Task<AdminCourseResponseModel> GetAdminCourse(int id);

        PaginationResponseModel<SmallCourseResponseModel> GetAll(PaginationRequestModel<CourseTableColumn> model);

    }
}