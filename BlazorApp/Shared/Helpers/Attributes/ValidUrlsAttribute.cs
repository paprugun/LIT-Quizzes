using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BlazorApp.Shared.Helpers.Attributes
{
    public class ValidUrlsAttribute : ValidationAttribute
    {
        public ValidUrlsAttribute(string errorMessage)
            : base(errorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string strValue = value.ToString();
                string[] urls = strValue.Split(' ');

                foreach (string url in urls)
                {
                    if (!IsValidUrl(url))
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }
            }

            return ValidationResult.Success;
        }

        private bool IsValidUrl(string url)
        {
            string urlPattern = @"^(http|https)://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,}";

            Regex regex = new Regex(urlPattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(url);
        }
    }
}
