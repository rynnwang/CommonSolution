using System;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// Class SentenceExtension
    /// </summary>
    public static class SentenceExtension
    {
        /// <summary>
        /// Splits the sentence by upper cases.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <returns></returns>
        public static string SplitSentenceByUpperCases(this string anyString)
        {
            return StringRegexExtension.SplitByUpperCases(anyString, StringConstants.WhiteSpace);
        }

        /// <summary>
        /// Combines the sentence.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="specialCharactors">The special charactors.</param>
        /// <param name="specialCharactorReplace">The special charactor replace.</param>
        /// <returns>System.String.</returns>
        public static string CombineSentence(this string sentence, char[] specialCharactors = null, char specialCharactorReplace = '_')
        {
            if (!string.IsNullOrWhiteSpace(sentence))
            {
                var builder = new StringBuilder(sentence.Length);

                foreach (var one in sentence.Split(new char[] { StringConstants.WhiteSpaceChar }, StringSplitOptions.RemoveEmptyEntries))
                {
                    builder.Append(one.Replace(specialCharactors, specialCharactorReplace));
                }

                return builder.ToString();
            }

            return sentence;
        }
    }
}