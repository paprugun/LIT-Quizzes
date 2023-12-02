using BlazorApp.Shared.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.RequestModels.Pagination
{
    public class PaginationRequestModel<T> : PaginationBaseRequestModel where T : struct
    {
        [JsonProperty("search")]
        public string Search { get; set; }

        [JsonProperty("order")]
        public OrderingRequestModel<T, SortingDirection> Order { get; set; }
    }
}
