using System;
using System.Text;

namespace ifunction
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
            var builder = new StringBuilder();

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
                            builder.Append(" ");
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
                            builder.Append(" ");
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
        /// Combines the sentence.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="specialCharactors">The special charactors.</param>
        /// <param name="specialCharactorReplace">The special charactor replace.</param>
        /// <returns></returns>
        public static string CombineSentence(this string sentence, char[] specialCharactors = null,
            char specialCharactorReplace = '_')
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(sentence))
            {
                foreach (var one in sentence.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    builder.Append(one.Replace(specialCharactors, specialCharactorReplace));
                }
            }

            return builder.ToString();
        }
    }
}
