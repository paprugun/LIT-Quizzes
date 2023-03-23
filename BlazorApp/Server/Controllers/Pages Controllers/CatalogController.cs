using AutoMapper;
using BlazorApp.Models.ResponseModels.Quiz;
using BlazorApp.ResourceLibrary;
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared.Models.ResponseModels.Quiz;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp.Server.Controllers.Pages_Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatalogController : _BaseApiController
    {
        private readonly IQuizAdminService _adminService;
        private readonly IMapper _mapper;

        public CatalogController(IStringLocalizer<ErrorsResource> errorsLocalizer, IQuizAdminService adminService, IMapper mapper) : base(errorsLocalizer)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IEnumerable<SmallQuizResponseModel>> GetCatalog() 
        {
            var response = await _adminService.GetAllSmallQuizzes();
            return response;
        }
    }
}
