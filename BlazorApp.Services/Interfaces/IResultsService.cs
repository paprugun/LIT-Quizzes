using BlazorApp.Models.ResponseModels.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces
{
    public interface IResultsService
    {
        Task<UserResultResponseModel> GetResult(int id);
        Task<IEnumerable<SmallUserResultResponseModel>> GetAllResults(int id);
        Task DeleteResult(int resultId);
    }
}
