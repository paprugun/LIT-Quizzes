using BlazorApp.Common.Attributes;
using BlazorApp.Common.Constants;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.Models.RequestModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels;
using BlazorApp.Models.ResponseModels.Base.CursorPagination;
using BlazorApp.Models.ResponseModels.Session;
using BlazorApp.ResourceLibrary;
using BlazorApp.Server.Helpers.Attributes;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.API.Admin
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiVersion}/admin-users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Role.Admins)]
    [Validate]
    public class AdminUsersController : _BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public AdminUsersController(IStringLocalizer<ErrorsResource> localizer,
            UserManager<ApplicationUser> userManager,
            IUserService userService)
             : base(localizer)
        {
            _userManager = userManager;
            _userService = userService;
        }

        // GET api/v1/admin-users
        /// <summary>
        /// Retrieve users in pagination
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/v1/admin-users?Search=xsdfadsf&amp;Order.Key=Id&amp;Order.Direction=Asc&amp;Limit=45&amp;Offset=45
        /// 
        /// </remarks>
        /// <param name="model">Pagination request model</param>
        /// <param name="getAdmins">get admins or not</param>
        /// <returns>HTTP 200 with users list in pagination or 40X, 500 with error message</returns>  
        [HttpPost]
        [SwaggerOperation(Tags = new[] { "Admin Users" })]
        [SwaggerResponse(200, ResponseMessages.RequestSuccessful, typeof(JsonPaginationResponse<List<UserTableRowResponseModel>>))]
        [SwaggerResponse(400, ResponseMessages.InvalidData, typeof(ErrorResponseModel))]
        [SwaggerResponse(401, ResponseMessages.Unauthorized, typeof(ErrorResponseModel))]
        [SwaggerResponse(403, ResponseMessages.Forbidden, typeof(ErrorResponseModel))]
        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
        public IActionResult GetAll([FromBody]PaginationRequestModel<UserTableColumn> model, [FromQuery] bool getAdmins)
        {
            var data = _userService.GetAll(model, getAdmins);

            return Json(new JsonPaginationResponse<List<UserTableRowResponseModel>>(data.Data, model.Offset + model.Limit, data.TotalCount));
        }

        // GET api/v1/admin-users/cursor
        /// <summary>
        /// Retrieve users in cursor pagination
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/v1/admin-users/cursor?Search=xsdfadsf&amp;Order.Key=Id&amp;Order.Direction=Asc&amp;Limit=45&amp;LastId=10
        /// 
        /// </remarks>
        /// <param name="model">Cursor pagination request model</param>
        /// <returns>HTTP 200 with users list in pagination or 40X, 500 with error message</returns>  
        [HttpGet("cursor")]
        [SwaggerOperation(Tags = new[] { "Admin Users" })]
        [SwaggerResponse(200, ResponseMessages.RequestSuccessful, typeof(CursorJsonPaginationResponse<List<UserTableRowResponseModel>>))]
        [SwaggerResponse(400, ResponseMessages.InvalidData, typeof(ErrorResponseModel))]
        [SwaggerResponse(401, ResponseMessages.Unauthorized, typeof(ErrorResponseModel))]
        [SwaggerResponse(403, ResponseMessages.Forbidden, typeof(ErrorResponseModel))]
        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
        public IActionResult GetAllByCursor([FromQuery] CursorPaginationRequestModel<UserTableColumn> model)
        {
            var data = _userService.GetAll(model);

            return Json(new CursorJsonPaginationResponse<List<UserTableRowResponseModel>>(data.Data, data.LastId));
        }

        [HttpGet("setrole/{id}-{role}")]
        public async Task SetRole([FromRoute] int id, [FromRoute] string role)
        {
            await _userService.SetRole(role, id);
        }

        [HttpDelete("deleterole/{id}-{role}")]
        public async Task DeleteRole([FromRoute] int id, [FromRoute] string role)
        {
            await _userService.DeleteRole(role, id);
        }

        [HttpGet("user/{id}")]
        public async Task<UserRoleResponseModel> GetUserInfo([FromRoute] int id)
        {
            var response = await _userService.GetUserInfo(id);
            return response;
        }

        // DELETE api/v1/admin-users/{id}
        /// <summary>
        /// Delete user
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE /api/v1/admin-users/2
        /// 
        /// </remarks>
        /// <param name="id">Id of user</param>
        /// <returns>HTTP 200 with user profile or 40X, 500 with error message</returns>   
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new[] { "Admin Users" })]
        [SwaggerResponse(200, ResponseMessages.RequestSuccessful, typeof(JsonResponse<UserResponseModel>))]
        [SwaggerResponse(400, ResponseMessages.InvalidData, typeof(ErrorResponseModel))]
        [SwaggerResponse(401, ResponseMessages.Unauthorized, typeof(ErrorResponseModel))]
        [SwaggerResponse(403, ResponseMessages.Forbidden, typeof(ErrorResponseModel))]
        [SwaggerResponse(500, ResponseMessages.InternalServerError, typeof(ErrorResponseModel))]
        public IActionResult Delete([ValidateId][FromRoute]int id)
        {
            //return Json(new JsonResponse<UserResponseModel>(_userService.SoftDeleteUser(id)));
            return Ok(_userService.HardDeleteUser(id));
        }

        [HttpGet("{id}")]
        public IActionResult Disban([ValidateId][FromRoute] int id)
        {
            return Json(new JsonResponse<UserResponseModel>(_userService.DisbanUser(id)));
        }

    }
}