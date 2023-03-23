using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {

        private readonly ILogger<NoteController> _logger;
        private readonly INoteService _noteService;

        public NoteController(ILogger<NoteController> logger,
            INoteService noteService)
        {
            _logger = logger;
            _noteService = noteService;
        }

        //save
        [HttpGet("save/{name}-{note}")]
        public async Task<Note> Save([FromRoute] string name, [FromRoute] string note)
        {
            var model = new Note()
            {
                Name = name,
                Text = note
            };

            var response = await _noteService.Create(model);

            return response;
        }

        //getAll
        [HttpGet("")]
        public async Task<Note> Save1([FromRoute] string name, [FromRoute] string note)
        {
            var model = new Note()
            {
                Name = name,
                Text = note
            };

            var response = await _noteService.Create(model);

            return response;
        }
        //get by name
        [HttpGet("{name}")]
        public async Task<Note> GetByName([FromRoute] string name, [FromRoute] string note)
        {
            var model = new Note()
            {
                Name = name,
                Text = note
            };

            var response = await _noteService.Create(model);

            return response;
        }

        //delete by name
        [HttpGet("{name}/delete")]
        public async Task<Note> DeleteByName([FromRoute] string name, [FromRoute] string note)
        {
            var model = new Note()
            {
                Name = name,
                Text = note
            };

            var response = await _noteService.Create(model);

            return response;
        }
    }
}
