using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Pagination.CursorPagination
{
    public class CursorJsonPaginationResponseModel<T> : JsonResponseModel<T> where T : class
    {
        public CursorJsonPaginationResponseModel(T newdata, int? lastId)
            : base(newdata)
        {

            Pagination = new CursorPagination()
            {
                LastId = lastId
            };

        }
        [JsonRequired]
        [JsonProperty("pagination")]
        public CursorPagination Pagination { get; set; }
    }

    public class CursorPagination
    {
        [JsonProperty("lastId")]
        public int? LastId { get; set; }
    }
}
