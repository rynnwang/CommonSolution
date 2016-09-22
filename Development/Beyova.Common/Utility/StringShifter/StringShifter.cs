using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    /// Class StringShifter. Example:
    /// <![CDATA[
    /// var shifter = new StringShifter("http://{feature}.test.com/{id}", "http://test.com/api/v1/{feature}/{id}/", new Dictionary<string, string> { { "id", "[0-9A-Za-z]+" } });
    /// var result = shifter.Shift("http://nav.test.com/nav1234/"); // result: http://test.com/api/v1/nav/nav1234/
    /// ]]>
    /// </summary>
    public sealed class StringShifter
    {
        /// <summary>
        /// The _source regex
        /// </summary>
        private Regex _sourceRegex;

        /// <summary>
        /// The _destination shift shard
        /// </summary>
        private StringShiftShard _destinationShiftShard;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringShifter" /> class.
        /// </summary>
        /// <param name="sourceFormat">The source format.</param>
        /// <param name="destinationFormat">The destination format.</param>
        /// <param name="sourceFormatConstraints">The source format constraints.</param>
        public StringShifter(string sourceFormat, string destinationFormat, Dictionary<string, string> sourceFormatConstraints = null)
        {
            _sourceRegex = sourceFormat.ConvertFormatToRegex(sourceFormatConstraints, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _destinationShiftShard = destinationFormat.ConvertFormatToShiftShard();
        }

        /// <summary>
        /// Shifts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <returns><c>true</c> if matched, <c>false</c> otherwise.</returns>
        public bool Shift(string input, out string output)
        {
            output = null;

            try
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    return false;
                }
                var match = _sourceRegex.Match(input);

                if (match.Success)
                {
                    output = _destinationShiftShard.ToString(match);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(input);
            }
        }

        /// <summary>
        /// Shifts the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>System.String.</returns>
        public string Shift(string input)
        {
            string output;
            Shift(input, out output);

            return output;
        }
    }
}
