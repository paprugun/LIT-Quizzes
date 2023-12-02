using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.RequestModels.Pagination
{
    public class PaginationBaseRequestModel
    {
        [JsonProperty("limit")]
        public int Limit { get; set; } = 10;

        [JsonProperty("offset")]
        public int Offset { get; set; } = 0;
    }
}
