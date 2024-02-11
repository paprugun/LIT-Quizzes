using Newtonsoft.Json;
using BlazorApp.Models.Enums;
using System.Collections.Generic;

namespace BlazorApp.Models.RequestModels
{
    public class PaginationRequestModel<T> : PaginationBaseRequestModel where T : struct
    {
        [JsonProperty("search")]
        public string Search { get; set; }

        [JsonProperty("filters")]
        public List<string> Filters { get; set; }

        [JsonProperty("order")]
        public OrderingRequestModel<T, SortingDirection> Order { get; set; }
    }
}
