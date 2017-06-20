using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Interface IPlaceHolderMember
    /// </summary>
    public interface IPlaceHolderMember
    {
        /// <summary>
        /// Gets the supported place keys.
        /// </summary>
        /// <value>
        /// The supported place keys.
        /// </value>
        ICollection<string> SupportedPlaceKeys { get; }

        /// <summary>
        /// Gets the place value by key.
        /// </summary>
        /// <param name="stringPlaceHolder">The string place holder.</param>
        /// <returns></returns>
        string GetPlaceValueByKey(StringPlaceHolder stringPlaceHolder);
    }
}