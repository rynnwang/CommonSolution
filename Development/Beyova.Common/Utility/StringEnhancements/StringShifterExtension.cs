using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    /// Class StringShifterExtension.
    /// </summary>
    public static class StringShifterExtension
    {
        /// <summary>
        /// The holder key variable
        /// </summary>
        private const string holderKeyVariable = "placeholder";

        /// <summary>
        /// The place holder regex
        /// </summary>
        private static Regex placeHolderRegex = new Regex(@"\{(?<" + holderKeyVariable + @">([a-zA-Z0-9_-]+))\}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The holder key result
        /// </summary>
        private const string holderKeyResult = "${" + holderKeyVariable + "}";

        /// <summary>
        /// The place holder pattern format
        /// </summary>
        private const string placeHolderPatternFormat = "(?<{0}>({1}))";

        /// <summary>
        /// The default constraint
        /// </summary>
        internal const string defaultConstraint = "(.)+";

        /// <summary>
        /// Gets the place holder pattern.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns>System.String.</returns>
        private static string GetPlaceHolderPattern(string variableName, string pattern)
        {
            return string.Format(placeHolderPatternFormat, variableName, pattern);
        }

        /// <summary>
        /// Creates the string builder.
        /// </summary>
        /// <param name="shard">The shard.</param>
        /// <returns>StringBuilder.</returns>
        internal static StringBuilder CreateStringBuilder(this StringShiftShard shard)
        {
            return shard == null ? null : new StringBuilder(GetStringBuilderDefaultCapacity(shard));
        }

        /// <summary>
        /// Gets the string builder default capacity.
        /// </summary>
        /// <param name="shard">The shard.</param>
        /// <returns>System.Int32.</returns>
        internal static int GetStringBuilderDefaultCapacity(this StringShiftShard shard)
        {
            return shard == null ? 0 : (shard.staticShards.Sum(x => x.Length) * 2);
        }

        /// <summary>
        /// Converts the format to regex. Like convert "Name: {name}" within constraints "[a-z0-9]+" to "Name:\s(?&lt;name&gt;([a-z0-9]+))" as <seealso cref="Regex" />. If no constraint specified, use ".+".
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="constraints">The constraints.</param>
        /// <param name="regexOptions">The regex options.</param>
        /// <returns>Regex.</returns>
        public static Regex ConvertFormatToRegex(this string format, Dictionary<string, string> constraints = null, RegexOptions regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase)
        {
            var shards = ConvertFormatToShiftShard(format);

            if (shards == null)
            {
                return null;
            }

            if (!shards.IsValid)
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(shards), shards);
            }

            var builder = shards.CreateStringBuilder();

            if (constraints == null)
            {
                constraints = new Dictionary<string, string>();
            }

            for (var i = 0; i < shards.dynamicShards.Count; i++)
            {
                builder.Append(shards.staticShards[i].StaticStringToRegexPattern());
                string constraintPattern;
                builder.Append(GetPlaceHolderPattern(shards.dynamicShards[i], constraints.TryGetValue(shards.dynamicShards[i], out constraintPattern) ? constraintPattern : defaultConstraint));
            }

            builder.Append(shards.staticShards[shards.dynamicShards.Count]);

            return new Regex(builder.ToString(), regexOptions);
        }

        /// <summary>
        /// Converts format to shift shards. Like convert "Its name is {name}" to <seealso cref="StringShiftShard"/> instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>StringShiftShard.</returns>
        public static StringShiftShard ConvertFormatToShiftShard(this string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                return null;
            }

            var matches = placeHolderRegex.Matches(format);

            StringShiftShard result = new StringShiftShard();
            var lastStart = 0;
            foreach (Match one in matches)
            {
                var staticPart = format.Substring(lastStart, one.Index - lastStart);
                result.staticShards.Add(staticPart);
                var holderKey = one.Result(holderKeyResult);
                result.dynamicShards.Add(one.Result(holderKeyResult));
                lastStart = lastStart + 2 + staticPart.Length + holderKey.Length;
            }

            result.staticShards.Add(format.Substring(lastStart, format.Length - lastStart));

            return result;
        }

        /// <summary>
        /// Finds the place holders. Given string "{name}'s birthday is {date}.", return ["name","date"]
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>System.String[].</returns>
        public static string[] FindPlaceHolders(this string anyString)
        {
            if (string.IsNullOrWhiteSpace(anyString))
            {
                return new string[] { };
            }

            var matches = placeHolderRegex.Matches(anyString);
            HashSet<string> result = new HashSet<string>();

            foreach (Match one in matches)
            {
                result.Add(one.Result(holderKeyResult));
            }

            return result.ToArray();
        }
    }
}