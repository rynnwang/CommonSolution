using System.Xml.Linq;

namespace ifunction
{
    /// <summary>
    /// Interface IXmlSerializable
    /// </summary>
    public interface IXmlSerializable
    {
        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns>XElement.</returns>
        XElement ToXml();

        /// <summary>
        /// Fills the property values by XML.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        void FillPropertyValuesByXml(XElement xElement);
    }
}
