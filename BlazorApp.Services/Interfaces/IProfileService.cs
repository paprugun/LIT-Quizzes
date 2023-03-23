using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels.Results;
using BlazorApp.Models.ResponseModels.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces
{
    public interface IProfileService
    {
        Task<IEnumerable<SmallUserResultResponseModel>> GetAllResults();
        Task<UserRoleResponseModel> Edit(UserProfileRequestModel model);
        Task<UserResultResponseModel> GetResult(int resultId);
        Task DeleteResult(int resultId);
        Task DeleteAccount();

    }
}
