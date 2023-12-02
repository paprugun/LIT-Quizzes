using BlazorApp.ResourceLibrary;
using BlazorApp.Server.Helpers.Attributes;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.API.Admin
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiVersion}/admin-verifications")]
    [Validate]
    public class AdminVerificationsController : _BaseApiController
    {
        private readonly IAccountService _accountService;

        public AdminVerificationsController(IStringLocalizer<ErrorsResource> errorsLocalizer, 
            IAccountService accountService)
            : base(errorsLocalizer)
        {
            _accountService = accountService;
        }

    }
}