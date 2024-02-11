using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.RequestModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels.Course;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.ResourceLibrary;
using BlazorApp.Server.Helpers.Attributes;
using BlazorApp.Services.Interfaces.CourseServices;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CoursesController : _BaseApiController
    {
        private readonly ICourseService _courseService;
        private readonly IUserCourseService _userCourseService;
        public CoursesController(IStringLocalizer<ErrorsResource> errorsLocalizer, 
                                ICourseService courseService,
                                IUserCourseService userCourseService) : base(errorsLocalizer)
        {
            _courseService = courseService;
            _userCourseService = userCourseService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromBody] PaginationRequestModel<CourseTableColumn> model) 
        {
            var response = _courseService.GetAll(model);
            return Json(new JsonPaginationResponse<List<SmallCourseResponseModel>>(response.Data, model.Offset + model.Limit, response.TotalCount));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var response = await _courseService.GetCourse(id);
            return Ok(new JsonResponse<CourseResponseModel>(response));
        }

        [HttpGet("assign/{id}")]
        public async Task<IActionResult> Assign([FromRoute] int id)
        {
            var response = _userCourseService.AssignCourse(id);
            if (response.IsCompletedSuccessfully)
                return Ok();
            return BadRequest();
        }

        [HttpPost("assignments")]
        public async Task<IActionResult> Assignments([FromBody] CursorPaginationRequestModel<CourseTableColumn> model)
        {
            var response = _userCourseService.GetAssignments(model);
            return Json(new CursorJsonPaginationResponse<List<UserCourseResultResponseModel>>(response.Data, response.LastId));
        }

        [HttpGet("get-assingment/{id}")]
        public async Task<IActionResult> GetAssingment([FromRoute] int id)
        {
            var response = await _userCourseService.GetAssignment(id);
            return Json(new JsonResponse<AssignmentResponseModel>(response));
        }
    }
}
