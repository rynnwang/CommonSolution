using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class JsonXmlizer, which is used for xmlizing JSON objects.
    /// </summary>
    public static class JsonXmlizer
    {
        /// <summary>
        /// The attribute type
        /// </summary>
        private const string attributeType = "Type";

        /// <summary>
        /// The node array
        /// </summary>
        private const string nodeArray = "Item";

        /// <summary>
        /// The node root
        /// </summary>
        private const string nodeRoot = "Root";

        /// <summary>
        /// The attribute name
        /// </summary>
        private const string attributeName = "name";

        /// <summary>
        /// The attribute version
        /// </summary>
        private const string attributeVersion = "version";

        /// <summary>
        /// The version value
        /// </summary>
        internal const string VersionValue = "v2";

        /// <summary>
        /// The node property
        /// </summary>
        private const string nodeProperty = "Property";

        /// <summary>
        /// The null j token
        /// </summary>
        public static readonly JToken NullJToken = JToken.Parse("null");

        /// <summary>
        /// The undefined j token
        /// </summary>
        public static readonly JToken UndefinedJToken = JToken.Parse("undefined");

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <param name="xElement">The x element.</param>
        /// <param name="toAdd">To add.</param>
        private static void AddIfNotNull(this XElement xElement, XElement toAdd)
        {
            if (xElement != null && toAdd != null)
            {
                xElement.Add(toAdd);
            }
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="jToken">The j token.</param>
        /// <param name="rootNodeName">Name of the node.</param>
        /// <param name="arrayItemName">Name of the array item.</param>
        /// <returns>XElement.</returns>
        public static XElement Xmlize(this JToken jToken, string rootNodeName = null, string arrayItemName = null)
        {
            StringBuilder builder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(builder, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                FillXml(writer, jToken, rootNodeName, arrayItemName);
            }

            var result = XElement.Parse(builder.ToString());
            result.SetAttributeValue(attributeVersion, VersionValue);

            return result;
        }

        /// <summary>
        /// To the XML string.
        /// </summary>
        /// <param name="jToken">The j token.</param>
        /// <param name="rootNodeName">Name of the node.</param>
        /// <param name="arrayItemName">Name of the array item.</param>
        /// <returns>System.String.</returns>
        public static string XmlizeToString(this JToken jToken, string rootNodeName = null, string arrayItemName = null)
        {
            return Xmlize(jToken, rootNodeName, arrayItemName).ToString();
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="jToken">The j token.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="arrayItemName">Name of the array item.</param>
        /// <param name="attributeNameValue">The attribute name value.</param>
        /// <returns>XElement.</returns>
        private static void FillXml(XmlWriter writer, JToken jToken, string nodeName = null, string arrayItemName = null, string attributeNameValue = null)
        {
            if (jToken != null)
            {
                //Node start
                writer.WriteStartElement(nodeName.SafeToString(nodeRoot));
                writer.WriteStartAttribute(attributeType);
                writer.WriteValue(jToken.Type.ToString());
                writer.WriteEndAttribute();

                if (!string.IsNullOrWhiteSpace(attributeNameValue))
                {
                    writer.WriteStartAttribute(attributeName);
                    writer.WriteValue(attributeNameValue);
                    writer.WriteEndAttribute();
                }

                switch (jToken.Type)
                {
                    case JTokenType.Array:
                        foreach (var one in jToken.Children())
                        {
                            FillXml(writer, one, arrayItemName.SafeToString(nodeArray));
                        }
                        break;
                    //Simple types
                    case JTokenType.Date:
                        writer.WriteValue(jToken.Value<DateTime>().ToFullDateTimeTzString());
                        break;
                    //Complex types
                    case JTokenType.Object:
                        foreach (var one in ((JObject)jToken).Properties())
                        {
                            FillXml(writer, one.Value, nodeProperty, attributeNameValue: one.Name);
                        }
                        break;

                    case JTokenType.TimeSpan:
                        writer.WriteValue(jToken.Value<TimeSpan>().TotalMilliseconds);
                        break;

                    case JTokenType.Property:
                    case JTokenType.Raw:
                    case JTokenType.String:
                    case JTokenType.Uri:
                    case JTokenType.Integer:
                    case JTokenType.Guid:
                    case JTokenType.Float:
                    case JTokenType.Boolean:
                        writer.WriteValue(jToken.ToString());
                        break;
                    //Following option should be appear without value being set.
                    case JTokenType.Null:
                    case JTokenType.Undefined:
                        break;
                    //Following option should be ignored.
                    case JTokenType.Comment:
                    case JTokenType.Constructor:
                    case JTokenType.None:
                    default:
                        break;
                }

                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Internals the dexmlize.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private static JToken InternalDexmlize(this XElement xml)
        {
            JToken result = null;
            JTokenType tokenType;

            if (Enum.TryParse<JTokenType>(xml.GetAttributeValue(attributeType), out tokenType))
            {
                if (xml != null)
                {
                    switch (tokenType)
                    {
                        case JTokenType.Array:
                            var array = new JArray();

                            foreach (var one in xml.Elements())
                            {
                                array.AddIfNotNull(InternalDexmlize(one));
                            }

                            result = array;
                            break;
                        //Simple types
                        case JTokenType.Date:
                            result = JToken.FromObject(Convert.ToDateTime(xml.Value));
                            break;

                        case JTokenType.Integer:
                        case JTokenType.Float:
                            result = JToken.Parse(xml.Value);
                            break;

                        case JTokenType.Boolean:
                            result = JToken.FromObject(xml.Value.ToBoolean(false));
                            break;
                        //Complex types
                        case JTokenType.Object:
                            var jObject = new JObject();
                            foreach (var one in xml.Elements())
                            {
                                if (one.Name.LocalName.Equals(nodeProperty, StringComparison.OrdinalIgnoreCase))
                                {
                                    var name = one.GetAttributeValue(attributeName);

                                    if (!string.IsNullOrWhiteSpace(name))
                                    {
                                        jObject.Add(name, InternalDexmlize(one));
                                    }
                                }
                            }
                            result = jObject;
                            break;

                        case JTokenType.Property:
                        case JTokenType.Raw:
                            break;

                        case JTokenType.TimeSpan:
                            result = JToken.FromObject(new TimeSpan(xml.Value.ToInt64()));
                            break;

                        case JTokenType.String:
                            result = JToken.FromObject(xml.Value);
                            break;

                        case JTokenType.Uri:
                            result = JToken.FromObject(new Uri(xml.Value));
                            break;

                        case JTokenType.Bytes:
                            result = JToken.FromObject(Encoding.UTF8.GetBytes(xml.Value));
                            break;

                        case JTokenType.Null:
                            result = NullJToken;
                            break;

                        case JTokenType.Undefined:
                            result = UndefinedJToken;
                            break;
                        //Following option should be ignored.
                        case JTokenType.Comment:
                        case JTokenType.Constructor:
                        case JTokenType.None:
                        default:
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Dexmlizes the specified XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>
        /// JToken.
        /// </returns>
        /// <exception cref="NotSupportedException"></exception>
        public static JToken Dexmlize(this XElement xml)
        {
            if (xml != null)
            {
                var version = xml.GetAttributeValue(attributeVersion);

                switch (version.ToLowerInvariant())
                {
                    case JsonXmlizer.VersionValue:
                        return InternalDexmlize(xml);

                    case "":
                    case JsonXmlSerializer.VersionValue:
                        return JsonXmlSerializer.InternalToJToken(xml);

                    default:
                        throw ExceptionFactory.CreateInvalidObjectException("version", version);
                }
            }

            return null;
        }
    }
}