using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Common.Utilities.Interfaces
{
    public interface IStateContainer<T> where T : class
    {
        T Value { get; set; }
        void SetValue(T value);
    }
}
