using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.RequestModels.Course;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Course;
using BlazorApp.ResourceLibrary;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.API.Admin
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admin)]
    public class AdminCoursesController : _BaseApiController
    {
        private readonly ICourseService _courseService;
        public AdminCoursesController(IStringLocalizer<ErrorsResource> errorsLocalizer, ICourseService courseService) : base(errorsLocalizer)
        {
            _courseService = courseService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CourseRequestModel model) 
        {
            var response = await _courseService.CreateCourse(model);
            return Created(new JsonResponse<CourseResponseModel>(response));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromBody] CourseRequestModel model, [FromRoute] int id) 
        {
            var response = await _courseService.UpdateCourse(model, id);
            return Ok(new JsonResponse<CourseResponseModel>(response));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _courseService.DeleteCourse(id);
            return Ok(new JsonResponse<MessageResponseModel>(new MessageResponseModel($"Course {id} was deleted")));
        }
    }
}
