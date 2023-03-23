using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Stats
{
    public class TopicsStatsResponseModel
    {
        public int TopicCount { get; set; }
        public int CSharpQuizzesCount { get; set; }
        public int JSQuizzesCount { get; set; }
        public int CPPQuizesCount { get; set; }
    }
}
