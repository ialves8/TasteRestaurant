using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasteRestaurant.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetPropertyValue<T>(this T item, string propertyNome)
        {
            return item.GetType().GetProperty(propertyNome).GetValue(item, null).ToString();
        }
    }
}
