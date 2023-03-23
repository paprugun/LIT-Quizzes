using System;
using Newtonsoft.Json;

namespace BlazorApp.Shared
{
    public class Note
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
