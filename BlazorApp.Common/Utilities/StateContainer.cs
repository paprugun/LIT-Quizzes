using BlazorApp.Common.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Common.Utilities
{
    public class StateContainer<T> : IStateContainer<T> where T : class
    {
        public T Value { get; set; }
        public event Action OnStateChange;
        public void SetValue(T value)
        {
            this.Value = value;
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
