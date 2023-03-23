using BlazorApp.Models.ResponseModels.MainPage;
using BlazorApp.Models.ResponseModels.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces.PageServices
{
    public interface IHomePageService
    {
        Task<IEnumerable<SmallQuizResponseModel>> GetFeaturesQuizzes();
        Task<CSharpCardInfo> CSCardInfo();
        Task<JSCardInfo> JSCardInfo();
        Task<CPPCardInfo> CPPCardInfo();

    }
}
