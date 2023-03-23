using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Stats
{
    public class TopicsStatsResponseModel
    {
        public int TopicCount { get; set; }
        public int CSharpQuizzesCount { get; set; }
        public int JSQuizzesCount { get; set; }
        public int CPPQuizesCount { get; set; }

    }
}
