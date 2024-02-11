using BlazorApp.Shared.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.ResponseModel.Course
{
    public class CourseRequestModel
    {
        [Required(ErrorMessage = "Поле 'Назва' є обов'язковим")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле 'Опис' є обов'язковим")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Поле 'URL контенту' є обов'язковим")]
        [ValidUrls("Поле 'URL контенту' повинно містити лише посилання")]
        public string ContentURLs { get; set; }

        [Required(ErrorMessage = "Поле 'Ідентифікатори тестів' є обов'язковим")]
        public int[] QuizzesIds { get; set; }

        [Range(1, 5, ErrorMessage = "Рівень складності повинен бути від 1 до 4")]
        public int Difficult { get; set; }

        [Required(ErrorMessage = "Поле 'Мова' є обов'язковим")]
        public string Language { get; set; }
    }
}
