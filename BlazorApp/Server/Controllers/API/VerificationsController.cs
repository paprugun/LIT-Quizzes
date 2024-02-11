using BlazorApp.Client.Pages.Admin;
using BlazorApp.Common.Utilities.Interfaces;
using BlazorApp.DAL.Abstract;
using BlazorApp.Domain.Entities.Identity;
using BlazorApp.Models.Enums;
using BlazorApp.Models.RequestModels;
using BlazorApp.ResourceLibrary;
using BlazorApp.Server.Helpers.Attributes;
using BlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.API
{
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{api-version:apiVersion}/[controller]")]
    public class VerificationsController : _BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashUtility _hashUtility;
        private readonly IEmailService _emailService;
        public VerificationsController(IStringLocalizer<ErrorsResource> errorsLocalizer, 
                                       IUnitOfWork unitOfWork,
                                       IHashUtility hashUtility,
                                       IEmailService emailService) : base(errorsLocalizer)
        {
            _unitOfWork = unitOfWork;
            _hashUtility = hashUtility;
            _emailService = emailService;
        }

        //api/v1/verifications/email?token={token}
        [HttpGet("email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token) 
        {
            var user = _unitOfWork.Repository<UserChangeRequest>().Get(x => x.TokenHash == _hashUtility.GetHash(token) && x.ChangeRequestType == ChangeRequestType.ConfirmEmail)
                                .Select(x => x.User)
                                .FirstOrDefault();

            user.EmailConfirmed = true;
            _unitOfWork.Repository<ApplicationUser>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Redirect($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/emailconfirmed");
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            await _emailService.Send(new MailRequestModel()
            {
                Addressee = "leguenkowork@gmail.com",
                Body = $"<p>Confirm email by clicking the link<a href=\"test url\">Confirm</a></p>",
                Subject = "Email Confirmation",
            });

            return Ok();
        }
    }
}
