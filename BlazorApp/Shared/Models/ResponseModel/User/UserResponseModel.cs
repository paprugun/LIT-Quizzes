using BlazorApp.Shared.Models.ResponseModel.Session;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.User
{
	public class UserResponseModel : UserBaseResponseModel
	{
		[JsonProperty("firstName")]
		public string FirstName { get; set; }

		[JsonProperty("lastName")]
		public string LastName { get; set; }

		[JsonProperty("isBlocked")]
		public bool IsBlocked { get; set; }

	}
}
