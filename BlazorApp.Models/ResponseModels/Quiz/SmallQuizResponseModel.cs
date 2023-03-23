using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Quiz
{
	public class SmallQuizResponseModel
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Topic { get; set; }
        public bool IsActive { get; set; }
        public int QuestionsCount { get; set; }
    }
}
