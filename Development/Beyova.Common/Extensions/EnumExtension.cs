using System;
using System.ComponentModel;
using System.Reflection;
using Beyova.ExceptionSystem;
using System.Linq;
using System.Collections;
using System.Globalization;

namespace Beyova
{
    /// <summary>
    /// Class EnumExtension.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Gets the enum default value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object GetEnumDefaultValue(Type type)
        {
            object defaultItem = null;

            if (type != null && type.IsEnum)
            {
                var enumValues = Enum.GetValues(type);

                foreach (var one in enumValues)
                {
                    if ((int)one == 0)
                    {
                        defaultItem = one;
                        break;
                    }
                }
            }

            return defaultItem;
        }

        /// <summary>
        /// Tries the parse enum.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="text">The text.</param>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static bool TryParseEnum(Type type, string text, out object enumValue, bool ignoreCase = false)
        {
            enumValue = null;
            return (type != null && type.IsEnum && !string.IsNullOrWhiteSpace(text)) ? InternalTryParseEnum(Enum.GetValues(type), text, out enumValue, ignoreCase) : false;
        }

        /// <summary>
        /// Internals the try parse enum.
        /// </summary>
        /// <param name="enumValues">The enum values.</param>
        /// <param name="text">The text.</param>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        internal static bool InternalTryParseEnum(Array enumValues, string text, out object enumValue, bool ignoreCase)
        {
            enumValue = null;

            if (enumValues != null && !string.IsNullOrWhiteSpace(text))
            {
                foreach (var one in enumValues)
                {
                    if (string.Compare(one.ToString(), text, ignoreCase) == 0)
                    {
                        enumValue = one;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified flag value has flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <param name="flagValue">The flag value.</param>
        /// <returns>
        ///   <c>true</c> if the specified flag value has flag; otherwise, <c>false</c>.
        /// </returns>
        public static bool EnumHasFlag<T>(this T enumValue, T flagValue)
            where T : struct, IConvertible
        {
            return (((IConvertible)enumValue).ToInt64(CultureInfo.InvariantCulture) & ((IConvertible)flagValue).ToInt64(CultureInfo.InvariantCulture)) > 0;
        }
    }
}
