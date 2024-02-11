using BlazorApp.Shared.Models.ResponseModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Course
{
    public class AdminCourseResponseModel : CourseResponseModel
    {
        public List<UserCourseResultResponseModel> UsersResults { get; set; }
    }
}
