using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Pagination
{
    public class PaginationResponse<T>
    {
        public PaginationResponse()
        {
        }

        public PaginationResponse(List<T> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public int TotalCount { get; set; }

        public List<T> Data { get; set; }
    }
}
