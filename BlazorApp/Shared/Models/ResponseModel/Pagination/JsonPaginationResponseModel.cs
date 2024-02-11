using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Pagination
{
    public class JsonPaginationResponse<T> : JsonResponse<T> where T : class
    {
        public JsonPaginationResponse(T newdata, int nextOffset, int totalCount)
            : base(newdata)
        {
            Pagination = new PaginationModel
            {
                NextOffset = nextOffset,
                TotalCount = totalCount
            };
        }

        [JsonRequired]
        [JsonProperty("pagination")]
        public PaginationModel Pagination { get; set; }
    }

    public class PaginationModel
    {
        /// <summary>
        /// request offset + length of returned array
        /// </summary>
        [JsonProperty("nextOffset")]
        [JsonRequired]
        public int NextOffset { get; set; }

        /// <summary>
        /// total count of items. This could be used for disabling endless scroll functionality
        /// </summary>
        [JsonProperty("totalCount")]
        [JsonRequired]
        public int TotalCount { get; set; }
    }
}
