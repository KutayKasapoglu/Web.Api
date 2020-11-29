using System;
using System.ComponentModel;
using System.Linq;

namespace Basket.Common.Helpers
{
    public static class EnumHelper
    {
        public static string GetEnumDescriptionForValue<T>(T value) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                // Enumeration'larda isimden Description okuma işlemi gerçekleştiriliyor
                // Enum'lar DI prensibiyle kullanılacak metot özelinde class'lara ayrılmıştır

                var enumType = typeof(T);
                foreach (T enumValue in Enum.GetValues(enumType))
                {
                    if (enumValue.Equals(value))
                    {
                        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
                        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        return attributes.Any() ? attributes[0].Description : Enum.GetName(enumType, enumValue);
                    }
                }
            }

            return "";
        }

        public static string GetEnumNameForValue<T>(int value) where T : struct, IConvertible
        {
            var text = "";
            if (typeof(T).IsEnum)
            {
                var enumType = typeof(T);
                foreach (var enumValue in Enum.GetValues(enumType))
                {
                    if ((int)enumValue == value)
                    {
                        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
                        text = Enum.GetName(enumType, enumValue);
                    }
                }
            }

            return text;
        }
    }
}
