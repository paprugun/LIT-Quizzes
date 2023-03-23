using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Pagination
{
    internal class JsonPaginationResponseModel<T> : JsonResponseModel<T> where T : class
    {
        public JsonPaginationResponseModel(T newdata, int nextOffset, int totalCount)
            : base(newdata)
        {
            Pagination = new Pagination
            {
                NextOffset = nextOffset,
                TotalCount = totalCount
            };
        }

        [JsonRequired]
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public class Pagination
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
