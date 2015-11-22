using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ifunction
{
    /// <summary>
    /// Extension class for string format validation.
    /// </summary>
    public static class StringRegexExtension
    {
        #region Regular Expression Extensions

        /// <summary>
        /// The start symbol
        /// </summary>
        const string startSymbol = "^";

        /// <summary>
        /// The end symbol
        /// </summary>
        const string endSymbol = "$";

        /// <summary>
        /// Starts the with regular expression string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="regularExpressionString">The regular expression string.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns><c>true</c> if given text starts with specific regular expression string, <c>false</c> otherwise.</returns>
        public static bool StartWithRegularExpressionString(this string text, string regularExpressionString, bool ignoreCase = false)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(regularExpressionString))
            {
                RegexOptions options = RegexOptions.Compiled;

                if (ignoreCase)
                {
                    options = options | RegexOptions.IgnoreCase;
                }

                var regex = new Regex(regularExpressionString.StartsWith(startSymbol) ? regularExpressionString : (startSymbol + regularExpressionString), options);
                result = regex.IsMatch(text);
            }

            return result;
        }

        /// <summary>
        /// Ends the with regular expression string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="regularExpressionString">The regular expression string.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns><c>true</c> if given text ends with specific regular expression string, <c>false</c> otherwise.</returns>
        public static bool EndWithRegularExpressionString(this string text, string regularExpressionString, bool ignoreCase = false)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(regularExpressionString))
            {
                RegexOptions options = RegexOptions.Compiled;

                if (ignoreCase)
                {
                    options = options | RegexOptions.IgnoreCase;
                }

                var regex = new Regex(regularExpressionString.EndsWith(endSymbol) ? regularExpressionString : (regularExpressionString + endSymbol), options);
                result = regex.IsMatch(text);
            }

            return result;
        }

        /// <summary>
        /// Splits the with regular expression string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="regularExpressionString">The regular expression string.</param>
        /// <param name="includeCapture">if set to <c>true</c> [include capture].</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>System.String[].</returns>
        public static string[] SplitWithRegularExpressionString(this string text, string regularExpressionString, bool includeCapture = false, bool ignoreCase = false)
        {
            string[] result = null;

            if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(regularExpressionString))
            {
                RegexOptions options = RegexOptions.Multiline | RegexOptions.Compiled;

                if (!includeCapture)
                {
                    options = options | RegexOptions.ExplicitCapture;
                }

                if (ignoreCase)
                {
                    options = options | RegexOptions.IgnoreCase;
                }

                var regex = new Regex(regularExpressionString, options);
                result = regex.Split(text);
            }

            return result;
        }

        /// <summary>
        /// To the regular expression.
        /// </summary>
        /// <param name="anyChar">Any character.</param>
        /// <returns>System.String.</returns>
        public static string ToRegularExpression(this char anyChar)
        {
            if ((anyChar >= 'a' && anyChar <= 'z') || (anyChar >= 'A' && anyChar <= 'Z'))
            {
                return string.Format("[{0}{1}]", Char.ToLowerInvariant(anyChar), Char.ToUpperInvariant(anyChar));
            }
            else if (anyChar == '{' || anyChar == '}' || anyChar == '(' || anyChar == ')' || anyChar == '[' ||
                     anyChar == ']' || anyChar == '\\' || anyChar == '.' || anyChar == '-' || anyChar == '+')
            {
                return string.Format("\\{0}", anyChar);
            }

            return anyChar.ToString();
        }

        /// <summary>
        /// To the regular expression.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>System.String.</returns>
        public static string ToRegularExpression(this string anyString)
        {
            var builder = new StringBuilder();
            foreach (var one in anyString.SafeToString())
            {
                builder.Append(ToRegularExpression(one));
            }

            return builder.ToString();
        }

        #endregion

        #region Format regex

        /// <summary>
        /// The cell phone regex
        /// </summary>
        private static Regex cellPhoneRegex = new Regex(@"^((((\+)[0-9]{2,4}(\s)?)?)([1-9][0-9]{5,10}))$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The email regex
        /// </summary>
        private static Regex emailRegex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The mac address regex
        /// </summary>
        private static Regex macAddressRegex = new Regex("^[0-9A-F]{2}-[0-9A-F]{2}-[0-9A-F]{2}-[0-9A-F]{2}-[0-9A-F]{2}-[0-9A-F]{2}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        /// <summary>
        /// Determines whether the specified string obj is number.
        /// </summary>
        /// <param name="stringObj">The string obj.</param>
        /// <param name="min">The min.</param>
        /// <returns><c>true</c> if the specified string obj is number; otherwise, <c>false</c>.</returns>
        public static bool IsNumeric(this string stringObj, decimal min = decimal.MinValue)
        {
            decimal result;
            return !string.IsNullOrWhiteSpace(stringObj) && decimal.TryParse(stringObj, out result) && result >= min;
        }

        /// <summary>
        /// Determines whether [is email address] [the specified string object].
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns><c>true</c> if [is email address] [the specified string object]; otherwise, <c>false</c>.</returns>
        public static bool IsEmailAddress(this string stringObject)
        {
            return !string.IsNullOrWhiteSpace(stringObject) && emailRegex.IsMatch(stringObject);
        }

        /// <summary>
        /// Determines whether [is cellphone] [the specified string object].
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns><c>true</c> if [is cell phone] [the specified string object]; otherwise, <c>false</c>.</returns>
        public static bool IsCellphone(this string stringObject)
        {
            return !string.IsNullOrWhiteSpace(stringObject) && cellPhoneRegex.IsMatch(stringObject);
        }

        /// <summary>
        /// Indexes the of chinese charset.
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns>System.Int32.</returns>
        public static int IndexOfChineseCharset(this string stringToCheck)
        {
            if (!string.IsNullOrWhiteSpace(stringToCheck))
            {
                var i = 0;
                foreach (var charsetCode in stringToCheck.Select(Convert.ToInt64))
                {
                    if (charsetCode >= 0x4e00 && charsetCode <= 0x9fa5)
                    {
                        return i;
                    }

                    i++;
                }
            }

            return -1;
        }

        /// <summary>
        /// Determines whether [contains chinese charset] [the specified string to check].
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns><c>true</c> if [contains chinese charset] [the specified string to check]; otherwise, <c>false</c>.</returns>
        public static bool ContainsChineseCharset(this string stringToCheck)
        {
            return stringToCheck.IndexOfChineseCharset() > -1;
        }

        /// <summary>
        /// Determines whether [is ip v4 address] [the specified ip string].
        /// </summary>
        /// <param name="ipString">The ip string.</param>
        /// <returns><c>true</c> if [is ip v4 address] [the specified ip string]; otherwise, <c>false</c>.</returns>
        public static bool IsIpV4Address(this string ipString)
        {
            if (!string.IsNullOrWhiteSpace(ipString))
            {
                var numbers = ipString.Split('.');
                if (numbers.Length == 4)
                {
                    foreach (string num in numbers)
                    {
                        try
                        {
                            var ip = Convert.ToInt32(num);
                            if (ip < 0 || ip > 255)
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is mac address] [the specified mac address].
        /// </summary>
        /// <param name="macAddress">The mac address.</param>
        /// <returns><c>true</c> if [is mac address] [the specified mac address]; otherwise, <c>false</c>.</returns>
        public static bool IsMacAddress(this string macAddress)
        {
            return !string.IsNullOrWhiteSpace(macAddress) && macAddressRegex.IsMatch(macAddress);
        }

        /// <summary>
        /// Replaces the specified old values.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="oldValues">The old values.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns></returns>
        public static string Replace(this string anyString, char[] oldValues, char newValue)
        {
            if (!string.IsNullOrEmpty(anyString))
            {
                anyString = oldValues.Aggregate(anyString, (current, one) => current.Replace(one, newValue));
            }

            return anyString;
        }

        /// <summary>
        /// Replaces the specified replacements.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="replacements">The replacements.</param>
        /// <returns></returns>
        public static string Replace(this string anyString, Dictionary<string, string> replacements)
        {
            if (!string.IsNullOrEmpty(anyString) && replacements != null)
            {
                anyString = replacements.Aggregate(anyString, (current, one) => current.Replace(one.Key, one.Value));
            }

            return anyString;
        }

        /// <summary>
        /// Replaces the specified old values.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="oldValues">The old values.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns></returns>
        public static string Replace(this string anyString, string[] oldValues, string newValue)
        {
            if (!string.IsNullOrEmpty(anyString))
            {
                anyString = oldValues.Aggregate(anyString, (current, one) => current.Replace(one, newValue));
            }

            return anyString;
        }

        /// <summary>
        /// Safes to upper.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>System.String.</returns>
        public static string SafeToUpper(this string anyString)
        {
            return string.IsNullOrWhiteSpace(anyString) ? anyString : anyString.ToUpperInvariant();
        }

        /// <summary>
        /// Safes to lower.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>System.String.</returns>
        public static string SafeToLower(this string anyString)
        {
            return string.IsNullOrWhiteSpace(anyString) ? anyString : anyString.ToLowerInvariant();
        }

        /// <summary>
        /// To the first upper.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>System.String.</returns>
        public static string ToFirstUpper(this string anyString)
        {
            if (!string.IsNullOrEmpty(anyString))
            {
                return Char.ToUpperInvariant(anyString[0]) + (anyString.Length > 1 ? anyString.Substring(1) : string.Empty);
            }

            return anyString;
        }

        /// <summary>
        /// Gets the top n character.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>System.String.</returns>
        public static string GetTopNCharacter(this string anyString, int maxLength, string suffix = " ... ")
        {
            anyString = anyString.SafeToString();
            maxLength = maxLength <= 0 ? anyString.Length : maxLength;

            var useSuffix = anyString.Length > (maxLength + suffix.SafeToString().Length);
            var length = useSuffix ? maxLength : anyString.Length;
            return string.Format("{0}{1}", anyString.Substring(0, length), useSuffix ? suffix : string.Empty);
        }

        /// <summary>
        /// To the regular case ignoring expression.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>System.String.</returns>
        public static string ToRegularCaseIgnoringExpression(this string anyString)
        {
            var builder = new StringBuilder();

            foreach (var one in anyString.SafeToString())
            {
                builder.AppendFormat("[{0}{1}]", char.ToUpperInvariant(one), char.ToLowerInvariant(one));
            }

            return builder.ToString();
        }

        /// <summary>
        /// To the culture information.
        /// </summary>
        /// <param name="cultureCode">The culture code.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>CultureInfo.</returns>
        public static CultureInfo ToCultureInfo(this string cultureCode, bool throwException = false)
        {
            try
            {
                return CultureInfo.GetCultureInfo(cultureCode);
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle("ToCultureInfo", cultureCode);
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Capitalizes the specified any string.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>System.String.</returns>
        public static string Capitalize(this string anyString)
        {
            if (string.IsNullOrWhiteSpace(anyString))
            {
                return anyString;
            }

            return Char.ToUpperInvariant(anyString[0]) + anyString.Substring(1);
        }

        /// <summary>
        /// Propers the specified any string.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>System.String.</returns>
        public static string Proper(this string anyString)
        {
            if (string.IsNullOrWhiteSpace(anyString))
            {
                return anyString;
            }

            return Char.ToLowerInvariant(anyString[0]) + anyString.Substring(1);
        }

        /// <summary>
        /// Subs the string before first match.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="firstMatch">The first match.</param>
        /// <returns>System.String.</returns>
        public static string SubStringBeforeFirstMatch(this string original, string firstMatch)
        {
            if (!string.IsNullOrWhiteSpace(original) && !string.IsNullOrWhiteSpace(firstMatch))
            {
                var index = original.IndexOf(firstMatch, StringComparison.Ordinal);
                if (index == 0)
                {
                    return string.Empty;
                }
                else if (index > 0)
                {
                    return original.Substring(0, index);
                }
            }

            return original;
        }

        /// <summary>
        /// Subs the string before first match.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="lastMatch">The last match.</param>
        /// <returns>System.String.</returns>
        public static string SubStringBeforeLastMatch(this string original, string lastMatch)
        {
            if (!string.IsNullOrWhiteSpace(original) && !string.IsNullOrWhiteSpace(lastMatch))
            {
                var index = original.LastIndexOf(lastMatch, StringComparison.Ordinal);
                if (index == 0)
                {
                    return string.Empty;
                }
                else if (index > 0)
                {
                    return original.Substring(0, index);
                }
            }

            return original;
        }

        /// <summary>
        /// Subs the string after first match.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="firstMatch">The first match.</param>
        /// <returns>System.String.</returns>
        public static string SubStringAfterFirstMatch(this string original, string firstMatch)
        {
            if (!string.IsNullOrWhiteSpace(original) && !string.IsNullOrWhiteSpace(firstMatch))
            {
                var index = original.IndexOf(firstMatch, StringComparison.Ordinal);
                if (index == original.Length)
                {
                    return string.Empty;
                }
                else if (index >= 0)
                {
                    return original.Substring(index);
                }
            }

            return original;
        }

        /// <summary>
        /// Subs the string after last match.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="lastMatch">The last match.</param>
        /// <returns>System.String.</returns>
        public static string SubStringAfterLastMatch(this string original, string lastMatch)
        {
            if (!string.IsNullOrWhiteSpace(original) && !string.IsNullOrWhiteSpace(lastMatch))
            {
                var index = original.LastIndexOf(lastMatch, StringComparison.Ordinal);
                if (index == original.Length)
                {
                    return string.Empty;
                }
                else if (index >= 0)
                {
                    return original.Substring(index);
                }
            }

            return original;
        }

        /// <summary>
        /// Subs the string before first match.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="firstMatch">The first match.</param>
        /// <returns>System.String.</returns>
        public static string SubStringBeforeFirstMatch(this string original, char firstMatch)
        {
            if (!string.IsNullOrWhiteSpace(original))
            {
                var index = original.IndexOf(firstMatch);
                if (index == 0)
                {
                    return string.Empty;
                }
                else if (index > 0)
                {
                    return original.Substring(0, index);
                }
            }

            return original;
        }

        /// <summary>
        /// Finds the common start string.
        /// </summary>
        /// <param name="string1">The string1.</param>
        /// <param name="string2">The string2.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>System.String.</returns>
        public static string FindCommonStartString(this string string1, string string2, bool ignoreCase = false)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(string1) && !string.IsNullOrWhiteSpace(string2))
            {
                if (ignoreCase)
                {
                    string1 = string1.ToLowerInvariant();
                    string2 = string2.ToLowerInvariant();
                }

                var length = string1.Length < string2.Length ? string1.Length : string2.Length;

                for (var i = 0; i < length; i++)
                {
                    if (string1[i] != string2[i])
                    {
                        if (i > 0)
                        {
                            return string1.Substring(0, i);
                        }
                    }
                }

                return string1.Substring(0, length);
            }

            return result;
        }

        #region StringBuilder

        /// <summary>
        /// Appends the indent.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="indentChar">The indent character.</param>
        /// <param name="amount">The amount.</param>
        public static void AppendIndent(this StringBuilder builder, char indentChar, int amount)
        {
            if (builder != null && amount > 0)
            {
                builder.Append(new string(indentChar, amount));
            }
        }

        /// <summary>
        /// Appends the indent.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="indentString">The indent string.</param>
        /// <param name="amount">The amount.</param>
        public static void AppendIndent(this StringBuilder builder, string indentString, int amount)
        {
            if (builder != null && amount > 0 && !string.IsNullOrEmpty(indentString))
            {
                for (var i = 0; i < amount; i++)
                {
                    builder.Append(indentString);
                }
            }
        }

        /// <summary>
        /// Appends the line with format.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void AppendLineWithFormat(this StringBuilder builder, string format, params object[] args)
        {
            if (builder != null && !string.IsNullOrWhiteSpace(format))
            {
                builder.AppendLine(string.Format(format, args));
            }
        }

        /// <summary>
        /// Appends the line with format.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="indentChar">The indent character.</param>
        /// <param name="indentAmount">The indent amount.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void AppendLineWithFormat(this StringBuilder builder, char indentChar, int indentAmount, string format, params object[] args)
        {
            if (builder != null && !string.IsNullOrWhiteSpace(format))
            {
                builder.AppendIndent(indentChar, indentAmount);
                builder.AppendLineWithFormat(format, args);
            }
        }

        /// <summary>
        /// Removes the last.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="count">The count.</param>
        public static StringBuilder RemoveLast(this StringBuilder builder, int count = 1)
        {
            if (builder != null && builder.Length >= count && count > 0)
            {
                builder = builder.Remove(builder.Length - count, count);
            }

            return builder;
        }

        /// <summary>
        /// The space lines
        /// </summary>
        static char[] spaceLines = new char[] { ' ', '\r', '\n', '\t' };

        /// <summary>
        /// Removes the last char if it equals to specific char from parameter.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="charToMatch">The character to match.</param>
        /// <param name="trimEndSpacesOrLines">if set to <c>true</c> [trim end spaces].</param>
        /// <returns>StringBuilder.</returns>
        public static StringBuilder RemoveLastIfMatch(this StringBuilder builder, char charToMatch, bool trimEndSpacesOrLines = false)
        {

            if (builder != null && builder.Length > 0)
            {
                if (trimEndSpacesOrLines)
                {
                    for (var i = builder.Length - 1; i >= 0; i--)
                    {
                        if (!spaceLines.Contains(builder[i]))
                        {
                            builder.Remove(i + 1, builder.Length - 1 - i);
                            break;
                        }
                    }
                }

                if (builder[builder.Length - 1].Equals(charToMatch))
                {
                    builder = builder.Remove(builder.Length - 1, 1);
                }
            }

            return builder;
        }

        /// <summary>
        /// Trims the end.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="charToTrim">The character to trim.</param>
        /// <returns>StringBuilder.</returns>
        public static void TrimEnd(this StringBuilder builder, params char[] charToTrim)
        {
            if (builder != null && builder.Length > 0)
            {
                charToTrim = (charToTrim == null || charToTrim.Length == 0) ? spaceLines : charToTrim;
                for (var i = builder.Length - 1; i >= 0; i--)
                {
                    if (!charToTrim.Contains(builder[i]))
                    {
                        builder.Remove(i + 1, builder.Length - 1 - i);
                        break;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Inners the string.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>System.String.</returns>
        public static string InnerString(this string anyString, string start, string end)
        {
            var result = anyString.SafeToString();

            start = start.SafeToString();
            end = end.SafeToString();

            int startIndex = start.Length == 0 ? 0 : result.IndexOf(start, StringComparison.Ordinal);
            if (startIndex < 0)
            {
                startIndex = 0;
            }

            var endIndex = end.Length == 0 ? result.Length : result.IndexOf(end, startIndex, StringComparison.Ordinal);
            if (endIndex < 0)
            {
                endIndex = result.Length;
            }

            result = result.Substring(startIndex + start.Length, endIndex - startIndex - start.Length);

            return result;
        }

        /// <summary>
        /// Wilds the card to regex.
        /// </summary>
        /// <param name="wildCard">The wild card.</param>
        /// <returns>Regex.</returns>
        public static Regex WildCardToRegex(this string wildCard)
        {
            if (!string.IsNullOrWhiteSpace(wildCard))
            {
                var pattern = wildCard.Replace(@"\", @"\\");
                pattern = pattern.Replace("(", @"\(");
                pattern = pattern.Replace(")", @"\)");
                pattern = pattern.Replace("[", @"\[");
                pattern = pattern.Replace("]", @"\]");
                pattern = pattern.Replace(".", @"\.");
                pattern = pattern.Replace("?", ".");
                pattern = pattern.Replace("*", ".*?");
                pattern = pattern.Replace(" ", @"\s");

                return new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }

            return null;
        }

        /// <summary>
        /// Safes the match.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="input">The input.</param>
        /// <returns>Match.</returns>
        public static Match SafeMatch(this Regex regex, string input)
        {
            return regex == null ? null : regex.Match(input);
        }

        /// <summary>
        /// Safes the matches.
        /// </summary>
        /// <param name="regex">The regex.</param>
        /// <param name="input">The input.</param>
        /// <returns>MatchCollection.</returns>
        public static MatchCollection SafeMatches(this Regex regex, string input)
        {
            return regex == null ? null : regex.Matches(input);
        }
    }
}
