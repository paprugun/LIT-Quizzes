using BlazorApp.Models.ResponseModels.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Session
{
    public class UserRoleResponse : UserResponse
    {
        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
