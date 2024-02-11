using BlazorApp.Models.ResponseModels.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces.CourseServices
{
    public interface IAdminCourseService
    {
        Task<AdminCourseResponseModel> Get(int courseId);
    }
}
