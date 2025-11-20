using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SaipemE_PTW.Shared.Models
{
    public static class EnumExtensions
    {
        public static DisplayAttribute? GetDisplayAttribute(this Enum value) =>
            value.GetType().GetMember(value.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>();

        public static string GetDisplayName(this Enum value) =>
            value.GetDisplayAttribute()?.GetName() ?? value.ToString();

        public static string GetShortName(this Enum value) =>
            value.GetDisplayAttribute()?.GetShortName() ?? value.ToString();

        public static string GetDescription(this Enum value) =>
            value.GetDisplayAttribute()?.GetDescription() ?? string.Empty;
    }
}
