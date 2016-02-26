using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Beyova
{
    /// <summary>
    /// Class for Serializable Dictionary, which is inherited from <see cref="Dictionary{T1,T2}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [XmlRoot("Dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, System.Xml.Serialization.IXmlSerializable
    {
        /// <summary>
        /// The XML_ root
        /// </summary>
        const string xml_Root = "Dictionary";

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        public SerializableDictionary()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        public SerializableDictionary(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public SerializableDictionary(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads the XML.
        /// </summary>
        /// <param name="xmlReader">The XML reader.</param>
        public void ReadXml(XmlReader xmlReader)
        {
            var keyXmlSerializer = new XmlSerializer(typeof(TKey));
            var valueXmlSerializer = new XmlSerializer(typeof(TValue));

            if (xmlReader.IsEmptyElement)
                return;

            xmlReader.ReadStartElement(xml_Root);

            while (xmlReader.NodeType != XmlNodeType.EndElement)
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    xmlReader.ReadStartElement(XmlConstants.node_Item);
                    xmlReader.ReadStartElement(XmlConstants.attribute_Key);

                    var key = (TKey)keyXmlSerializer.Deserialize(xmlReader);

                    xmlReader.ReadEndElement();
                    xmlReader.ReadStartElement(XmlConstants.attribute_Value);
                    var value = (TValue)valueXmlSerializer.Deserialize(xmlReader);
                    xmlReader.ReadEndElement();

                    this.Add(key, value);
                    xmlReader.ReadEndElement();
                }
                else
                {
                    xmlReader.Read();
                }
            }

            xmlReader.ReadEndElement();
        }

        /// <summary>
        /// Fills the by XML.
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        public void FillByXml(string xmlString)
        {
            try
            {
                using (var stringReader = new StringReader(xmlString))
                {
                    using (var xmlTextReader = new XmlTextReader(stringReader))
                    {
                        ReadXml(xmlTextReader);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("FillByXml", xmlString);
            }
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public void WriteXml(XmlWriter xmlWriter)
        {
            var keyXmlSerializer = new XmlSerializer(typeof(TKey));
            var valueXmlSerializer = new XmlSerializer(typeof(TValue));

            foreach (var key in this.Keys)
            {
                xmlWriter.WriteStartElement(XmlConstants.node_Item);

                xmlWriter.WriteStartElement(XmlConstants.attribute_Key);
                keyXmlSerializer.Serialize(xmlWriter, key);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement(XmlConstants.attribute_Value);
                var value = this[key];
                valueXmlSerializer.Serialize(xmlWriter, value);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
        }

        /// <summary>
        /// Serializes to string.
        /// </summary>
        /// <returns>System.String.</returns>
        public string ToXmlString()
        {
            try
            {
                var memoryStream = new MemoryStream();
                var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8) { Namespaces = true };

                WriteXml(xmlTextWriter);

                xmlTextWriter.Close();
                memoryStream.Close();

                var result = Encoding.UTF8.GetString(memoryStream.GetBuffer());
                var index = result.IndexOf(Convert.ToChar(60));
                if (index > -1)
                {
                    result = result.Substring(index);
                    result = result.Substring(0, (result.LastIndexOf(Convert.ToChar(62)) + 1));
                }
                else
                {
                    result = result.Trim('\0');
                }
                result = "<" + xml_Root + ">" + result + "</" + xml_Root + ">";

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle("ToXmlString");
            }
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <returns>XElement.</returns>
        public XElement ToXml()
        {
            return XElement.Parse(ToXmlString());
        }

        /// <summary>
        /// Fills the by XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        public void FillByXml(XElement xml)
        {
            if (xml != null)
            {
                FillByXml(xml.ToString());
            }
        }
    }
}
