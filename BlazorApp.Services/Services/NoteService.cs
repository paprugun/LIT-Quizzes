using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared;

namespace BlazorApp.Services.Services
{
    public class NoteService : INoteService
    {
        private List<Note> _notes { get; set; }

        public NoteService()
        {
            _notes = new List<Note>();
        }

        //add to note
        public async Task<Note> Create(Note model)
        {
            _notes.Add(model);
            var response = model;
            return response;
        }

        //delete from note by name
        public async Task Delete(string name)
        {
            _notes.Remove(await Get(name));
        }

        //get from note by name
        public async Task<Note> Get(string name)
        {
            var response = _notes.FirstOrDefault(x=>x.Name == name);
            return response;
        }

        //get note list
        public async Task<IEnumerable<Note>> GetAll()
        {
            var response = _notes;
            return response;
        }

    }
}
