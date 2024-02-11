using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.RequestModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces.CourseServices
{
    public interface IUserCourseService
    {
        Task AssignCourse(int courseId);
        Task<AssignmentResponseModel> GetAssignment(int courseId);
        PaginationResponseModel<SmallCourseResponseModel> GetAssignments(PaginationRequestModel<CourseTableColumn> model);
        CursorPaginationBaseResponseModel<UserCourseResultResponseModel> GetAssignments(CursorPaginationRequestModel<CourseTableColumn> model);
    }
}
