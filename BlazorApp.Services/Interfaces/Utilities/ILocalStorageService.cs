using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Services.Interfaces.Utilities
{
    public interface ILocalStorageService<T> where T : class
    {
		/// <summary>
		/// Remove a key from browser local storage.
		/// </summary>
		/// <param name="key">The key previously used to save to local storage.</param>
		public Task RemoveAsync(string key);

		/// <summary>
		/// Save a string value to browser local storage.
		/// </summary>
		/// <param name="key">The key to use to save to and retrieve from local storage.</param>
		/// <param name="value">The string value to save to local storage.</param>
		public Task SaveAsync(string key, string value);

		/// <summary>
		/// Get a string value from browser local storage.
		/// </summary>
		/// <param name="key">The key previously used to save to local storage.</param>
		/// <returns>The string previously saved to local storage.</returns>
		public Task<T> GetAsync(string key);

		/// <summary>
		/// Save an array of string values to browser local storage.
		/// </summary>
		/// <param name="key">The key previously used to save to local storage.</param>
		/// <param name="values">The array of string values to save to local storage.</param>
		public Task SaveArrayAsync(string key, string[] values);

		/// <summary>
		/// Get an array of string values from browser local storage.
		/// </summary>
		/// <param name="key">The key previously used to save to local storage.</param>
		/// <returns>The array of string values previously saved to local storage.</returns>
		public Task<IEnumerable<T>> GetArrayAsync(string[] key);
	}
}
