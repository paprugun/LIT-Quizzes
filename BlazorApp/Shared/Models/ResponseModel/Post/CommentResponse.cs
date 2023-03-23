using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Post
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public int ParentCommentId { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public bool isVisibleAnswers { get; set; }

        public List<CommentResponse> AnswersToComment { get; set; }
    }
}
