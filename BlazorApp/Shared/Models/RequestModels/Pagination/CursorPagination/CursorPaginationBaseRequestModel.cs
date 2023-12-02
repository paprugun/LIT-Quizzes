using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.RequestModels.Pagination.CursorPagination
{
    internal class CursorPaginationBaseRequestModel
    {
        [JsonProperty("limit")]
        public int Limit { get; set; } = 10;

        [JsonProperty("lastId")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} is invalid")]
        public int? LastId { get; set; }
    }
}
