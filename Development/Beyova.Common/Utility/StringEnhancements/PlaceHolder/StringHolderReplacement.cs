using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Beyova
{
    /// <summary>
    /// Class StringHolderReplacement.
    /// </summary>
    public static class StringHolderReplacement
    {
        /// <summary>
        /// Delegate TranslateValueDelegate
        /// </summary>
        /// <param name="stringPlaceHolder">The string place holder.</param>
        /// <param name="objects">The objects.</param>
        /// <returns>System.String.</returns>
        public delegate string TranslateValueDelegate(StringPlaceHolder stringPlaceHolder, params object[] objects);

        #region private fields

        /// <summary>
        /// The place holder sign format
        /// </summary>
        private const string placeHolderSignFormat = @"\[@{0}{1}\]";

        /// <summary>
        /// The plain place holder sign format
        /// </summary>
        private const string plainPlaceHolderSignFormat = "[@{0}{1}]";

        /// <summary>
        /// The holder key variable
        /// </summary>
        private const string holderKeyVariable = "Key";

        /// <summary>
        /// The holder key result
        /// </summary>
        private const string holderKeyResult = "${" + holderKeyVariable + "}";

        /// <summary>
        /// The additional parameter variable
        /// </summary>
        private const string additionalParameterVariable = "Parameters";

        /// <summary>
        /// The additional parameter result
        /// </summary>
        private const string additionalParameterResult = "${" + additionalParameterVariable + "}";

        /// <summary>
        /// The key regex
        /// </summary>
        private const string keyRegex = @"\w+";

        /// <summary>
        /// The additional parameter regex
        /// </summary>
        private const string additionalParameterRegex = @"\([a-z0-9A-Z@\?=#,&;:/\._'""\-\+]+\)";

        /// <summary>
        /// The place holder syntax regex
        /// </summary>
        private static readonly Regex placeHolderSyntaxRegex = new Regex(string.Format(placeHolderSignFormat, @"(?<" + holderKeyVariable + ">(" + keyRegex + "))", @"((?<" + additionalParameterVariable + ">(" + additionalParameterRegex + "))?)"),
             RegexOptions.Compiled);

        /// <summary>
        /// The place holder statement regex
        /// </summary>
        private static readonly Regex placeHolderStatementRegex = new Regex("(?<" + holderKeyVariable + ">(\\[@(" + keyRegex + ")((" + additionalParameterRegex + ")?\\])))", RegexOptions.Compiled);

        #endregion private fields

        #region Public static methods.

        /// <summary>
        /// Generates the place holder sign.
        /// </summary>
        /// <param name="placeKey">The place key.</param>
        /// <returns>The sign string for specific <c>placeKey</c>.
        /// <example>
        /// e.g.: [@UserName]
        /// </example></returns>
        public static string GeneratePlaceHolderSign(string placeKey)
        {
            return GeneratePlaceHolderSign(placeKey, null);
        }

        /// <summary>
        /// Generates the place holder sign.
        /// <remarks>
        /// <example>
        /// e.g.: [@UserName], [@Parameter(1)], etc.
        /// </example>
        /// </remarks>
        /// </summary>
        /// <param name="placeKey">The place key.</param>
        /// <param name="additionalParameterArray">The additional parameters.</param>
        /// <returns>The sign string for specific <c>placeKey</c> and <c>additionalParameters</c>.
        /// </returns>
        public static string GeneratePlaceHolderSign(string placeKey, params string[] additionalParameterArray)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(placeKey))
            {
                string additionalParameters = (additionalParameterArray == null || additionalParameterArray.Length == 0) ?
                    string.Empty : additionalParameterArray.Join(",");

                if (!string.IsNullOrWhiteSpace(additionalParameters))
                {
                    additionalParameters = "(" + additionalParameters + ")";
                }

                result = string.Format(plainPlaceHolderSignFormat, placeKey, additionalParameters);
            }

            return result;
        }

        /// <summary>
        /// Translates the place holder sign.
        /// </summary>
        /// <param name="statement">The statement.</param>
        /// <returns>The <see cref="StringPlaceHolder" /> instance if the input statement is valid for translating.</returns>
        public static StringPlaceHolder TranslatePlaceHolderSign(string statement)
        {
            StringPlaceHolder holder = null;

            if (!string.IsNullOrWhiteSpace(statement))
            {
                Match match = placeHolderSyntaxRegex.Match(statement);
                if (match.Success)
                {
                    holder = new StringPlaceHolder { PlaceKey = match.Result(holderKeyResult).ObjectToString() };
                    holder.AdditionalParameters.AddRange(match.Result(additionalParameterResult).ObjectToString().TrimStart('(').TrimEnd(')').Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }

            return holder;
        }

        /// <summary>
        /// Figures the content of the place keys in.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>ICollection{System.String}.</returns>
        public static ICollection<string> FigurePlaceKeysInContent(string content)
        {
            // Hashset keeps item in unique.
            HashSet<string> result = new HashSet<string>();

            if (!string.IsNullOrWhiteSpace(content))
            {
                MatchCollection matches = placeHolderSyntaxRegex.Matches(content);
                if (matches != null && matches.Count > 0)
                {
                    foreach (Match one in matches)
                    {
                        var key = one.Result(holderKeyResult).ObjectToString();
                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            result.Add(key);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Figures the content of the string place holders in.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>ICollection{StringPlaceHolder}.</returns>
        public static ICollection<StringPlaceHolder> FigureStringPlaceHoldersInContent(string content)
        {
            ICollection<StringPlaceHolder> result = new List<StringPlaceHolder>();

            if (!string.IsNullOrWhiteSpace(content))
            {
                MatchCollection matches = placeHolderStatementRegex.Matches(content);
                if (matches != null && matches.Count > 0)
                {
                    foreach (Match one in matches)
                    {
                        var key = one.Result(holderKeyResult).ObjectToString();

                        if (!string.IsNullOrWhiteSpace(key))
                        {
                            StringPlaceHolder holder = TranslatePlaceHolderSign(key);
                            if (holder != null && !result.Contains(holder))
                            {
                                result.Add(holder);
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Replaces the content.
        /// </summary>
        /// <param name="originalContent">Content of the original.</param>
        /// <param name="translateValueDelegate">The translate value delegate.</param>
        /// <param name="objects">The objects.</param>
        /// <returns>System.String.</returns>
        public static string ReplaceContent(string originalContent, TranslateValueDelegate translateValueDelegate = null, params object[] objects)
        {
            string result = originalContent;

            if (!string.IsNullOrWhiteSpace(originalContent))
            {
                if (translateValueDelegate == null)
                {
                    translateValueDelegate = StandardTranslateValueDelegate;
                }

                var holders = FigureStringPlaceHoldersInContent(originalContent);

                if (holders != null && holders.Count > 0)
                {
                    foreach (var one in holders)
                    {
                        result = ReplaceContent(result, one, translateValueDelegate, objects);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Replaces the content.
        /// </summary>
        /// <param name="originalContent">Content of the original.</param>
        /// <param name="placeHolder">The place holder.</param>
        /// <param name="translateValueDelegate">The translate value delegate.</param>
        /// <param name="objects">The objects.</param>
        /// <returns>System.String.</returns>
        public static string ReplaceContent(string originalContent, StringPlaceHolder placeHolder, TranslateValueDelegate translateValueDelegate, params object[] objects)
        {
            return (string.IsNullOrWhiteSpace(originalContent) || placeHolder == null) ? originalContent : originalContent.Replace(placeHolder.ToStatement(), translateValueDelegate(placeHolder, objects));
        }

        /// <summary>
        /// Standards the replace delegate.
        /// </summary>
        /// <param name="stringPlaceHolder">The string place holder.</param>
        /// <param name="objects">The objects.</param>
        /// <returns>System.String.</returns>
        private static string StandardTranslateValueDelegate(StringPlaceHolder stringPlaceHolder, params object[] objects)
        {
            string result = string.Empty;

            if (stringPlaceHolder != null && !string.IsNullOrWhiteSpace(stringPlaceHolder.PlaceKey)
                && objects != null && objects.Length > 0)
            {
                foreach (IPlaceHolderMember one in objects)
                {
                    if (one != null && one.SupportedPlaceKeys != null
                        && one.SupportedPlaceKeys.Contains(stringPlaceHolder.PlaceKey))
                    {
                        result = one.GetPlaceValueByKey(stringPlaceHolder);
                        break;
                    }
                }
            }

            return result;
        }

        #endregion Public static methods.
    }
}