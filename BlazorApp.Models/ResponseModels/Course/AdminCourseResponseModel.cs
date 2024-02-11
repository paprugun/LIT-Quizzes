using BlazorApp.Models.ResponseModels.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Course
{
    public class AdminCourseResponseModel : CourseResponseModel
    {
        public List<UserCourseResultResponseModel> UsersResults { get; set; }
    }
}
