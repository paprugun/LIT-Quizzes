using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Post
{
    public class PostResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public List<CommentResponse> Comments { get; set; } = new List<CommentResponse>();
    }
}
