using BlazorApp.Models.RequestModels.CursorPagination;
using BlazorApp.Models.ResponseModels.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces.PageServices
{
    public interface ICatalogPageService
    {
        Task<CatalogPaginationResponseModel> GetCatalog(CatalogPaginationRequestModel model);
    }
}
