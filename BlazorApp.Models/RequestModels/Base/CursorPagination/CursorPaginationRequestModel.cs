using Newtonsoft.Json;
using BlazorApp.Models.Enums;

namespace BlazorApp.Models.RequestModels.Base.CursorPagination
{
    public class CursorPaginationRequestModel<T> : CursorPaginationBaseRequestModel where T : struct
    {
        [JsonProperty("search")]
        public string Search { get; set; }

        [JsonProperty("order")]
        public OrderingRequestModel<T, SortingDirection> Order { get; set; }
    }
}
