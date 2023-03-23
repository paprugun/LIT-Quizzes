using BlazorApp.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.RequestModels.User
{
    public class EmailRequest
    {
        [Required(ErrorMessage = "Email address field is empty")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email address is not in valid format")]
        [StringLength(129, ErrorMessage = "Email address is not in valid format", MinimumLength = 3)]
        [CustomRegularExpression("([a-zA-Z0-9]+([+=#-._][a-zA-Z0-9]+)*@([a-zA-Z0-9]+(-[a-zA-Z0-9]+)*)+(([.][a-zA-Z0-9]{2,4})*)?)", ErrorMessage = "Email address is not in valid format")]
        [CustomRegularExpression("(^.{1,64}@.{1,64}$)", ErrorMessage = "Email address is not in valid format")]
        public string Email { get; set; }
    }
}
