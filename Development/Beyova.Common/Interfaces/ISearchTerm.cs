using System;

namespace Beyova
{
    /// <summary>
    /// Interface ISearchTerm
    /// </summary>
    public interface ISearchTerm
    {
        /// <summary>
        /// Gets or sets the search term. Full search term with alphabets only (English or Chinese Pinyin, etc.)
        /// </summary>
        /// <value>
        /// The search term.
        /// </value>
        string SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the short search term. Short search term with alphabets only (First alphabets of English or Chinese Pinyin, etc.)
        /// </summary>
        /// <value>
        /// The short search term.
        /// </value>
        string ShortSearchTerm { get; set; }
    }
}