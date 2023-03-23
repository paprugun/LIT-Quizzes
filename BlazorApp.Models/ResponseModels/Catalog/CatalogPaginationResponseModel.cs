using BlazorApp.Models.ResponseModels.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.Catalog
{
    public class CatalogPaginationResponseModel
    {
        public List<SmallQuizResponseModel> Quizzes { get; set; }
    }
}
