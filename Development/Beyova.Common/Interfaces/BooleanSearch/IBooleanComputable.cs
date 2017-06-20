using Newtonsoft.Json.Linq;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Interface IBooleanComputable
    /// </summary>
    public interface IBooleanComputable
    {
        /// <summary>
        /// Computes the specified json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        bool Compute(JObject json);
    }
}