using System;
using System.Xml.Linq;

namespace ifunction
{
    /// <summary>
    /// Interface IToXml
    /// </summary>
    [Obsolete("Use IXmlSerializable instead")]
    public interface IToXml : IXmlSerializable
    {
    }
}
