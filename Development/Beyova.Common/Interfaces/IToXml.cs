using System;
using System.Xml.Linq;

namespace Beyova
{
    /// <summary>
    /// Interface IToXml
    /// </summary>
    [Obsolete("Use IXmlSerializable instead")]
    public interface IToXml : IXmlSerializable
    {
    }
}
