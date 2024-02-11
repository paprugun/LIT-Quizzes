using Newtonsoft.Json;
using System.Collections.Generic;

namespace BlazorApp.Client.Shared
{
	public class ErrorResponseModel
	{
        [JsonRequired]
        [JsonProperty("_v")]
        public string Version { get; set; } = "1.0";

        [JsonRequired]
        [JsonProperty("code")]
        public string Code { get; set; }

        //[JsonProperty("message")]
        //public string Message { get; set; }

        [JsonProperty("stacktrace")]
        public string StackTrace { get; set; }

        [JsonProperty("errors")]
        public List<ErrorKeyValue> Errors { get; set; }
    }
    public class ErrorKeyValue
    {
        public ErrorKeyValue(string key, string msg)
        {
            Key = key;
            Message = msg;
        }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
