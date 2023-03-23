using Newtonsoft.Json;
using BlazorApp.Models.Enums;

namespace BlazorApp.Models.RequestModels
{
    public class PaginationRequestModel<T> : PaginationBaseRequestModel where T : struct
    {
        [JsonProperty("search")]
        public string Search { get; set; }

        [JsonProperty("order")]
        public OrderingRequestModel<T, SortingDirection> Order { get; set; }
    }
}
