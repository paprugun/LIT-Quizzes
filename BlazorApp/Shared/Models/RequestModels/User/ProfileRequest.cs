using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.RequestModels.User
{
    public class ProfileRequest
    {
        [Required(ErrorMessage = "Ім`я не може бути пустим")]
        [StringLength(30, ErrorMessage = "Ім`я має знаходитись в межах від 1 до 30 сивмолів", MinimumLength = 1)]
        [RegularExpression(@"(^\S*$)", ErrorMessage = "Ім`я не може містити пробіл")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Прізвище не може бути пустим")]
        [StringLength(30, ErrorMessage = "Прізвище має знаходитись в межах від 1 до 30 сивмолів", MinimumLength = 1)]
        [RegularExpression(@"(^\S*$)", ErrorMessage = "Прізвище не може містити пробіл")]
        public string LastName { get; set; }

        public int? ImageId { get; set; }
    }
}
