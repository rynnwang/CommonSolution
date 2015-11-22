using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace ifunction
{
    /// <summary>
    /// Interface IJsonSerializable
    /// </summary>
    public interface IJsonSerializable
    {
        /// <summary>
        /// To the JToken.
        /// </summary>
        /// <returns>JToken.</returns>
        JToken ToJToken();

        /// <summary>
        /// Fills the property values by JToken.
        /// </summary>
        /// <param name="jToken">The j token.</param>
        void FillPropertyValuesByJToken(JToken jToken);
    }
}
