using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NetCoreApp.Crosscutting.Helpers
{
    public static class AttributesHelper
    {
        public static IEnumerable<T> GetAttributes<T>(this object value) where T : Attribute
        {
            var type = value?.GetType();

            return (IEnumerable<T>)(value != null ?
                type.IsEnum ? type?.GetField(value?.ToString()).GetCustomAttributes(typeof(T)) : type?.GetCustomAttributes(typeof(T))
                : null);
        }

        public static string GetDescription(this object value)
        {
            return value.GetAttributes<DescriptionAttribute>()?.FirstOrDefault()?.Description;
        }
    }
}
