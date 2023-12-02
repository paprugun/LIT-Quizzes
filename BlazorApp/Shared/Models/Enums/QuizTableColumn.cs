using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum QuizTableColumn
    {
        Id,
        Name,
        Author,
        Topic,
        IsActive,
        QuestionsCount
    }
}
