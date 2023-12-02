using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Pagination.CursorPagination
{
    public class CursorPaginationBaseResponseModel<T>
    {
        public CursorPaginationBaseResponseModel() { }

        public CursorPaginationBaseResponseModel(List<T> data, int? lastId)
        {
            Data = data;
            LastId = lastId;
        }

        public int? LastId { get; set; }

        public List<T> Data { get; set; }
    }
}
