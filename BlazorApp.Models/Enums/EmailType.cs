using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlazorApp.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmailType
    {
        SuccessfulRegistration,
        ConfrimEmail,
        ResetPassword,
        TestEmail
    }
}
