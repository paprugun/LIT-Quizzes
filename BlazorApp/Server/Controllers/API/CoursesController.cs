using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Course;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.ResourceLibrary;
using BlazorApp.Server.Helpers.Attributes;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.API
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    public class CoursesController : _BaseApiController
    {
        private readonly ICourseService _courseService;
        public CoursesController(IStringLocalizer<ErrorsResource> errorsLocalizer, ICourseService courseService) : base(errorsLocalizer)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequestModel<CourseTableColumn> model) 
        {
            var response = _courseService.GetAll(model);
            return Json(new JsonPaginationResponse<List<SmallCourseResponseModel>>(response.Data, model.Offset + model.Limit, response.TotalCount));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var response = await _courseService.GetCourse(id);
            return Ok(new JsonResponse<CourseResponseModel>(response));
        }

        [HttpGet("assign/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Assign([FromRoute] int id)
        {
            var response = _courseService.AssignCourse(id);
            if (response.IsCompletedSuccessfully)
                return Ok();
            return BadRequest();
        }

        [HttpPost("assignments")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Assignments([FromBody] PaginationRequestModel<CourseTableColumn> model)
        {
            var response = _courseService.GetAssignments(model);
            return Json(new JsonPaginationResponse<List<SmallCourseResponseModel>>(response.Data, model.Offset + model.Limit, response.TotalCount));
        }

        [HttpGet("get-assingment/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAssigment([FromRoute] int id)
        {
            var response = await _courseService.GetAssignment(id);
            return Json(new JsonResponse<AssignmentResponseModel>(response));
        }
    }
}
