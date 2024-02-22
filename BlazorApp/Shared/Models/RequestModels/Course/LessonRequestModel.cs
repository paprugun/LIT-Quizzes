using BlazorApp.Shared.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Models.RequestModels.Course
{
    public class LessonRequestModel
    {
        [Required]
        public int CourseId { get; set; }

        [StringLength(100, ErrorMessage = "Назва уроку не більше 50 символів")]
        [Required(ErrorMessage = "Ім'я уроку обов'зкове поле")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Поле 'URL контенту' є обов'язковим")]
        [ValidUrls("Поле 'URL контенту' повинно містити лише посилання")]
        public string URL { get; set; }

        public double Time { get; set; }

        public List<int> QuizzesIds { get; set; }

    }
}
