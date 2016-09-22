using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Beyova.ExceptionSystem;
using Beyova;

namespace Beyova
{
    /// <summary>
    /// Extension class for collection type object.
    /// </summary>
    public static class DataExtension
    {
        /// <summary>
        /// Writes the CSV row.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="rowItems">The row items.</param>
        public static void WriteCsvRow(this StringBuilder stringBuilder, params string[] rowItems)
        {
            if (stringBuilder != null)
            {
                foreach(var one in rowItems)
                {
                    stringBuilder.Append(one);
                    stringBuilder.Append(StringConstants.CommaChar);
                }

                stringBuilder.RemoveLastIfMatch(StringConstants.CommaChar);
                stringBuilder.Append(StringConstants.NewLineChar);
            }
        }
    }
}
