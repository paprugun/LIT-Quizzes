using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.RequestModels.Course;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Course;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces
{
    public interface ICourseService
    {
        Task AssignCourse(int courseId);
        Task<AssignmentResponseModel> GetAssignment(int courseId);
        PaginationResponseModel<SmallCourseResponseModel> GetAssignments(PaginationRequestModel<CourseTableColumn> model);

        Task<CourseResponseModel> CreateCourse(CourseRequestModel model);

        Task<CourseResponseModel> UpdateCourse(CourseRequestModel model, int id);

        Task DeleteCourse(int id);

        Task<CourseResponseModel> GetCourse(int id);

        PaginationResponseModel<SmallCourseResponseModel> GetAll(PaginationRequestModel<CourseTableColumn> model);

    }
}