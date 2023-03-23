using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.RequestModels.CursorPagination
{
    public class CatalogPaginationRequestModel
    {
        public int? LastId { get; set; }
        public string? Search { get; set; }
    }
}
