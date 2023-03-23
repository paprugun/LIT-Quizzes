using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Pagination
{
    public class JsonResponseModel<T>
    {
        public JsonResponseModel(T newdata)
        {
            Data = newdata;
        }

        [JsonRequired]
        [JsonProperty("_v")]
        public string Version { get; set; } = "1.0";

        //[JsonRequired]
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
