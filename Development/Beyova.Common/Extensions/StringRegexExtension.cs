﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Beyova
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
        private const string startSymbol = "^";

        /// <summary>
        /// The end symbol
        /// </summary>
        private const string endSymbol = "$";

        /// <summary>
        /// Ases the quoted string.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>System.String.</returns>
        public static string AsQuotedString(this string anyString)
        {
            return string.IsNullOrWhiteSpace(anyString) ? string.Empty : string.Format("\"{0}\"", anyString);
        }

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

        #endregion Regular Expression Extensions

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

        #endregion Format regex

        #region Trim

        /// <summary>
        /// Internals the trim.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="trimStart">if set to <c>true</c> [trim start].</param>
        /// <param name="trimEnd">if set to <c>true</c> [trim end].</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>System.String.</returns>
        private static string InternalTrim(string source, string factor, bool trimStart, bool trimEnd, bool ignoreCase)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(factor))
            {
                return source;
            }

            int end = source.Length - 1;
            int start = 0;
            string tmp = source;
            CharComparer comparer = ignoreCase ? CharComparer.OrdinalIgnoreCase : CharComparer.Ordinal;

            if (trimStart)
            {
                bool isBreak = false;
                for (start = 0; start < tmp.Length; start += factor.Length)
                {
                    for (var i = 0; i < factor.Length; i++)
                    {
                        if (!comparer.Equals(factor[i], source[start + i]))
                        {
                            isBreak = true;
                            break;
                        }
                    }

                    if (isBreak)
                    {
                        break;
                    }
                }
            }

            if (trimEnd)
            {
                bool isBreak = false;
                for (end = tmp.Length - 1; end >= start; end -= factor.Length)
                {
                    for (var i = 0; i < factor.Length; i++)
                    {
                        if (!comparer.Equals(factor[factor.Length - 1 - i], source[start + i]))
                            if (factor[factor.Length - 1 - i] != source[end - i])
                            {
                                isBreak = true;
                                break;
                            }
                    }

                    if (isBreak)
                    {
                        break;
                    }
                }
            }

            return source.Substring(start, end - start + 1);
        }

        /// <summary>
        /// Trims the end.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>System.String.</returns>
        public static string TrimEnd(this string anyString, string factor, bool ignoreCase = false)
        {
            return InternalTrim(anyString, factor, false, true, ignoreCase);
        }

        /// <summary>
        /// Trims the start.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>System.String.</returns>
        public static string TrimStart(this string anyString, string factor, bool ignoreCase = false)
        {
            return InternalTrim(anyString, factor, true, false, ignoreCase);
        }

        /// <summary>
        /// Trims the specified any string.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>System.String.</returns>
        public static string Trim(this string anyString, string factor, bool ignoreCase = false)
        {
            return InternalTrim(anyString, factor, true, true, ignoreCase);
        }

        #endregion Trim

        /// <summary>
        /// As secure string.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>SecureString.</returns>
        public static SecureString AsSecureString(this string anyString)
        {
            if (string.IsNullOrEmpty(anyString))
            {
                return null;
            }

            var result = new SecureString();
            foreach (var one in anyString)
            {
                result.AppendChar(one);
            }

            return result;
        }

        /// <summary>
        /// Ensures the start with.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>System.String.</returns>
        public static string EnsureStartWith(this string anyString, string factor)
        {
            return (string.IsNullOrEmpty(anyString) || string.IsNullOrEmpty(factor)) ? anyString : (factor + anyString.TrimStart(factor));
        }

        /// <summary>
        /// Ensures the end with.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="factor">The factor.</param>
        /// <returns>System.String.</returns>
        public static string EnsureEndWith(this string anyString, string factor)
        {
            return (string.IsNullOrEmpty(anyString) || string.IsNullOrEmpty(factor)) ? anyString : (anyString.TrimEnd(factor) + factor);
        }

        /// <summary>
        /// Determines whether [contains any of] [the specified items].
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="items">The items.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>
        ///   <c>true</c> if [contains any of] [the specified items]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsAnyOf(this string sourceString, IEnumerable<string> items, bool ignoreCase = false)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(sourceString) && items.HasItem())
            {
                foreach (var one in items)
                {
                    if (sourceString.IndexOf(one, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) > -1)
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Likes the specified term.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="term">The term.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static bool Like(this string sourceString, string term, bool ignoreCase = false)
        {
            return !string.IsNullOrEmpty(sourceString) && !string.IsNullOrEmpty(term) &&
                (ignoreCase ?
                    sourceString.ToLowerInvariant().IndexOf(term.ToLowerInvariant()) > -1
                    : sourceString.IndexOf(term) > -1);
        }

        /// <summary>
        /// Splits the by upper cases.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        public static string SplitByUpperCases(this string anyString, string seperator = StringConstants.WhiteSpace)
        {
            var builder = new StringBuilder(anyString.Length * 2);

            if (seperator == null)
            {
                seperator = StringConstants.WhiteSpace;
            }

            if (!string.IsNullOrEmpty(anyString))
            {
                var start = 0;
                var isInShortTerm = false;
                var lastIsUpperCase = Char.IsUpper(anyString[0]);

                for (var i = 1; i < anyString.Length; i++)
                {
                    if (Char.IsUpper(anyString[i]))
                    {
                        if (!lastIsUpperCase)
                        {
                            builder.Append(anyString.Substring(start, i - start));
                            builder.Append(seperator);
                            start = i;
                        }
                        else
                        {
                            isInShortTerm = true;
                        }

                        lastIsUpperCase = true;
                    }
                    else
                    {
                        if (isInShortTerm)
                        {
                            builder.Append(anyString.Substring(start, i - start - 1));
                            builder.Append(seperator);
                            start = i - 1;
                        }
                        isInShortTerm = false;
                        lastIsUpperCase = false;
                    }
                }

                builder.Append(anyString.Substring(start));
            }

            return builder.ToString().TrimEnd();
        }

        /// <summary>
        /// To the hexadecimal string.
        /// </summary>
        /// <param name="anyGuid">Any unique identifier.</param>
        /// <returns>System.String.</returns>
        public static string ToHexString(this Guid? anyGuid)
        {
            return anyGuid.HasValue ? anyGuid.Value.ToHexString() : string.Empty;
        }

        /// <summary>
        /// To the hexadecimal string.
        /// </summary>
        /// <param name="anyGuid">Any unique identifier.</param>
        /// <returns>System.String.</returns>
        public static string ToHexString(this Guid anyGuid)
        {
            return anyGuid.ToString("N");
        }

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
        /// <returns>System.String.</returns>
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
        /// <returns>System.String.</returns>
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
        /// <param name="maxLength">The maximum length. The length does not contain suffix.</param>
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
                    throw ex.Handle(cultureCode);
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
        /// Propers the specified any string. Capitalizes the first letter in a text string and any other letters in text that follow any character other than a letter. Converts all other letters to lowercase letters.
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
        /// <param name="comparison">The comparison.</param>
        /// <returns>System.String.</returns>
        public static string SubStringBeforeFirstMatch(this string original, string firstMatch, StringComparison comparison = StringComparison.Ordinal)
        {
            if (!string.IsNullOrWhiteSpace(original) && !string.IsNullOrWhiteSpace(firstMatch))
            {
                var index = original.IndexOf(firstMatch, comparison);
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
        /// <param name="comparison">The comparison.</param>
        /// <returns>System.String.</returns>
        public static string SubStringBeforeLastMatch(this string original, string lastMatch, StringComparison comparison = StringComparison.Ordinal)
        {
            if (!string.IsNullOrWhiteSpace(original) && !string.IsNullOrWhiteSpace(lastMatch))
            {
                var index = original.LastIndexOf(lastMatch, comparison);
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
        /// <param name="comparison">The comparison.</param>
        /// <returns>System.String.</returns>
        public static string SubStringAfterFirstMatch(this string original, string firstMatch, StringComparison comparison = StringComparison.Ordinal)
        {
            if (!string.IsNullOrWhiteSpace(original) && !string.IsNullOrWhiteSpace(firstMatch))
            {
                var index = original.IndexOf(firstMatch, comparison);
                if (index == (original.Length - 1))
                {
                    return string.Empty;
                }
                else if (index >= 0)
                {
                    return original.Substring(index + 1);
                }
            }

            return original;
        }

        /// <summary>
        /// Subs the string after last match.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="lastMatch">The last match.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns>System.String.</returns>
        public static string SubStringAfterLastMatch(this string original, string lastMatch, StringComparison comparison = StringComparison.Ordinal)
        {
            if (!string.IsNullOrWhiteSpace(original) && !string.IsNullOrWhiteSpace(lastMatch))
            {
                var index = original.LastIndexOf(lastMatch, comparison);
                if (index == (original.Length - 1))
                {
                    return string.Empty;
                }
                else if (index >= 0)
                {
                    return original.Substring(index + 1);
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
        /// Subs the string after first match.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="afterMatch">The after match.</param>
        /// <returns>System.String.</returns>
        public static string SubStringAfterFirstMatch(this string original, char afterMatch)
        {
            if (!string.IsNullOrWhiteSpace(original))
            {
                var index = original.IndexOf(afterMatch);
                if (index == (original.Length - 1))
                {
                    return string.Empty;
                }
                else if (index >= 0)
                {
                    return original.Substring(index + 1);
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
        public static string FindCommonStartSubString(this string string1, string string2, bool ignoreCase = false)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(string1) && !string.IsNullOrWhiteSpace(string2))
            {
                CharComparer charComparer = ignoreCase ? CharComparer.OrdinalIgnoreCase : CharComparer.Ordinal;

                var length = string1.Length < string2.Length ? string1.Length : string2.Length;

                for (var i = 0; i < length; i++)
                {
                    if (charComparer.Equals(string1[i], string2[i]))
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
        /// Appends the line.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="indent">The indent.</param>
        /// <param name="value">The value.</param>
        public static void AppendLine(this StringBuilder builder, int indent, string value)
        {
            if (builder != null)
            {
                builder.AppendIndent(indent);
                builder.AppendLine(value);
            }
        }

        /// <summary>
        /// Appends the indent.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="indent">The indent.</param>
        public static void AppendIndent(this StringBuilder builder, int indent)
        {
            if (builder != null && indent > 0)
            {
                builder.Append(new string(StringConstants.WhiteSpaceChar, indent * 4));
            }
        }

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
        [Obsolete]
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
        /// <param name="indentAmount">The indent amount.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void AppendLineWithFormat(this StringBuilder builder, int indentAmount, string format, params object[] args)
        {
            if (builder != null && !string.IsNullOrWhiteSpace(format))
            {
                builder.AppendIndent(indentAmount);
                builder.AppendLineWithFormat(format, args);
            }
        }

        /// <summary>
        /// Appends the with format.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="indentAmount">The indent amount.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void AppendWithFormat(this StringBuilder builder, int indentAmount, string format, params object[] args)
        {
            if (builder != null && !string.IsNullOrWhiteSpace(format))
            {
                builder.AppendIndent(indentAmount);
                builder.Append(string.Format(format, args));
            }
        }

        /// <summary>
        /// Appends the with format.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        public static void AppendWithFormat(this StringBuilder builder, string format, params object[] args)
        {
            if (builder != null && !string.IsNullOrWhiteSpace(format))
            {
                builder.Append(string.Format(format, args));
            }
        }

        /// <summary>
        /// Get last N char. If given count is larger than builder count, return string.Empty.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static string Last(this StringBuilder builder, int count = 1)
        {
            if (builder != null && builder.Length >= count && count > 0)
            {
                return builder.ToString(builder.Length - count, count);
            }

            return string.Empty;
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
        private static char[] spaceLines = new char[] { ' ', '\r', '\n', '\t' };

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

                if (builder.Length > 0 && builder[builder.Length - 1].Equals(charToMatch))
                {
                    builder = builder.Remove(builder.Length - 1, 1);
                }
            }

            return builder;
        }

        /// <summary>
        /// Removes the last if match.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="stringToMatch">The string to match.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <param name="trimEndSpacesOrLines">if set to <c>true</c> [trim end spaces or lines].</param>
        /// <returns></returns>
        public static StringBuilder RemoveLastIfMatch(this StringBuilder builder, string stringToMatch, StringComparison stringComparison = StringComparison.Ordinal, bool trimEndSpacesOrLines = false)
        {
            if (builder != null && !string.IsNullOrEmpty(stringToMatch) && builder.Length > stringToMatch.Length)
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

                if (builder.Length > stringToMatch.Length && builder.Last(stringToMatch.Length).Equals(stringToMatch, stringComparison))
                {
                    builder = builder.RemoveLast(stringToMatch.Length);
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

        #endregion StringBuilder

        /// <summary>
        /// Inners the string.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="comparison">The comparison.</param>
        /// <returns>System.String.</returns>
        public static string InnerString(this string anyString, string start, string end, StringComparison comparison = StringComparison.Ordinal)
        {
            var result = anyString.SafeToString();

            start = start.SafeToString();
            end = end.SafeToString();

            int startIndex = start.Length == 0 ? 0 : result.IndexOf(start, comparison);
            if (startIndex < 0)
            {
                startIndex = 0;
            }

            var endIndex = end.Length == 0 ? result.Length : result.IndexOf(end, startIndex, comparison);
            if (endIndex < 0)
            {
                endIndex = result.Length;
            }

            result = result.Substring(startIndex + start.Length, endIndex - startIndex - start.Length);

            return result;
        }

        /// <summary>
        /// Converts static string to regex pattern.
        /// </summary>
        /// <param name="staticString">The static string.</param>
        /// <returns>System.String.</returns>
        public static string StaticStringToRegexPattern(this string staticString)
        {
            if (!string.IsNullOrEmpty(staticString))
            {
                var pattern = staticString.Replace(@"\", @"\\")
                    .Replace("/", @"\/")
                    .Replace("(", @"\(")
                    .Replace(")", @"\)")
                    .Replace("[", @"\[")
                    .Replace("]", @"\]")
                    .Replace(".", @"\.")
                    .Replace("-", @"\-")
                    .Replace("?", @"\?")
                    .Replace("*", @"\*")
                    .Replace(" ", @"\s");

                return pattern;
            }

            return string.Empty;
        }

        /// <summary>
        /// Converts wild card string to regex.
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

        /// <summary>
        /// Safes the format.
        /// </summary>
        /// <param name="formatString">The format string.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>System.String.</returns>
        public static string SafeFormat(this string formatString, params object[] args)
        {
            return string.IsNullOrWhiteSpace(formatString) ? formatString : string.Format(formatString, args);
        }

        #region Regex convert

        /// <summary>
        /// To the regex shad.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns>System.String.</returns>
        public static string ToRegexShad(this string anyString)
        {
            if (!string.IsNullOrEmpty(anyString))
            {
                return anyString.Replace("\\", "\\\\")
                    .Replace(@" ", @"\s")
                    .Replace("(", @"\(")
                    .Replace(")", @"\)")
                    .Replace("[", @"\[")
                    .Replace("]", @"\]")
                    .Replace("{", @"\{")
                    .Replace("}", @"\}")
                    .Replace(".", @"\.")
                    .Replace("?", @"\?")
                    .Replace("+", @"\+")
                    .Replace("^", @"\^")
                    .Replace("$", @"\$")
                    .Replace("!", @"\!")
                    .Replace("~", @"\~")
                    .Replace("*", @"\*");
            }

            return string.Empty;
        }

        #endregion Regex convert
    }
}