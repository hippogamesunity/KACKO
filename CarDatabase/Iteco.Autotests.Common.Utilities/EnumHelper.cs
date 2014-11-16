using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Iteco.Autotests.Common.Utilities
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            var fieldName = value.ToString();
            var fieldInfo = value.GetType().GetField(fieldName);
            var attributes = (DescriptionAttribute[]) fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : fieldName;
        }

        public static T GetEnumByDescription<T>(string description)
        {
            Contract.Requires(!string.IsNullOrEmpty(description));
            Contract.Requires(typeof(T).IsEnum);

            foreach (var value in Enum.GetValues(typeof(T)).Cast<T>().Where(value => GetEnumDescription((Enum)(object)value) == description))
            {
                return value;
            }

            throw new Exception();
        }
    }
}