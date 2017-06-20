using System.Text;

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
                foreach (var one in rowItems)
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