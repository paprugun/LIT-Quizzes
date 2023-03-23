using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Shared;

namespace BlazorApp.Services.Interfaces
{
    public interface INoteService
    {
        //save
        Task<Note> Create(Note model);

        //getAll
        Task<IEnumerable<Note>> GetAll();

        //get by name
        Task<Note> Get(string name);

        //delete by name
        Task Delete(string name);
    }
}
