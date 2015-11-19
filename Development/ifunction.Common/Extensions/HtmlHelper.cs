using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using ifunction.BinaryStorage;

namespace ifunction
{
    /// <summary>
    /// Class HtmlHelper.
    /// </summary>
    public static class HtmlHelper
    {
        /// <summary>
        /// Booleans the value to property value.
        /// <remarks>This method is to create html prop value. Like selected="selected" or checked="checked"</remarks>
        /// </summary>
        /// <param name="booleanValue">if set to <c>true</c> [boolean value].</param>
        /// <param name="propValue">The property value.</param>
        /// <returns></returns>
        public static string BooleanValueToPropValue(this bool booleanValue, string propValue)
        {
            return (string.IsNullOrWhiteSpace(propValue) || !booleanValue)
                ? string.Empty
                : string.Format(" {0}=\"{0}\" ", propValue);
        }
    }
}
