using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.ResponseModels.User
{
    public class UserResponse
    {
        [JsonProperty("userId")]
        public int Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("isBlocked")]
        public bool IsBlocked { get; set; }
    }
}
