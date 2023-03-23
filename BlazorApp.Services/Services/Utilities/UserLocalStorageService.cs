using BlazorApp.Common.Exceptions;
using BlazorApp.Services.Interfaces.Utilities;
using BlazorApp.Shared.Models.ResponseModel.Session;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace BlazorApp.Services.Services.Utilities
{
    public class UserLocalStorageService : ILocalStorageService<UserRoleResponse>
    {
        private readonly ILocalStorageService _localStorage;

        public UserLocalStorageService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<IEnumerable<UserRoleResponse>> GetArrayAsync(string[] keys)
        {
            var response = new List<UserRoleResponse>();
            foreach (var key in keys)
            {
                var data = await GetAsync(key);
                if (data == null)
                    throw new CustomException(System.Net.HttpStatusCode.BadRequest, "invalid key", "invalid storage key");
                response.Add(data);
            }
            return response;
        }

        public async Task<UserRoleResponse> GetAsync(string key)
        {
            var response = await _localStorage.GetItemAsync<UserRoleResponse>(key);
            if (response == null)
                throw new CustomException(System.Net.HttpStatusCode.BadRequest, "invalid key", "invalid storage key");
            return response;
        }

        public async Task RemoveAsync(string key)
        {
            await _localStorage.RemoveItemAsync(key);
        }

        public async Task SaveArrayAsync(string key, string[] values)
        {
            
        }

        public async Task SaveAsync(string key, string value)
        {
            await _localStorage.SetItemAsync(key, value);
        }
    }
}
