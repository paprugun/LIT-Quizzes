using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property,
        AllowMultiple = true, Inherited = true)]
    public class CustomRegularExpressionAttribute : RegularExpressionAttribute
    {
        private object _typeId = new object();
        public override object TypeId
        {
            get { return _typeId; }
        }

        public CustomRegularExpressionAttribute(string pattern) : base(pattern)
        {
        }
    }
}
