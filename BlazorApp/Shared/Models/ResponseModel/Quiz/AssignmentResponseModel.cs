using BlazorApp.Shared.Models.ResponseModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Quiz
{
    public class AssignmentResponseModel
    {
        public int Id { get; set; }

        public string CourseName { get; set; }

        public string[] Topics { get; set; }

        public string Language { get; set; }

        public string ContentURLs { get; set; }

        public List<UserResultResponse> Results { get; set; }

        public List<int> NotFinishedQuizzesIds { get; set; }
    }
}
