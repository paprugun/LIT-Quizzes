using System.Collections.Generic;

namespace BlazorApp.Common.Extensions
{
    public static class ListExtensions
    {
        public static List<TResult> Empty<TResult>(this List<TResult> list)
        {
            return new List<TResult>();
        }
    }
}
