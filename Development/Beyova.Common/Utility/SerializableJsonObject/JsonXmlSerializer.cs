using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Xml;

namespace Beyova
{
    /// <summary>
    /// Class JsonXmlSerializer, which is used for converting between Json and Xml
    /// </summary> 
    public static class JsonXmlSerializer
    {
        /// <summary>
        /// The version value
        /// </summary>
        internal const string VersionValue = "v1";

        /// <summary>
        /// The node name prefix
        /// </summary>
        private const string nodeNamePrefix = "__";

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
        /// The null JToken
        /// </summary>
        public static readonly JToken NullJToken = JToken.Parse("null");

        /// <summary>
        /// The undefined JToken
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
        /// <param name="jToken">The JToken.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="arrayItemName">Name of the array item.</param>
        /// <returns>XElement.</returns>
        [Obsolete("Use JsonXmlizer.Xmlize instead.", true)]
        public static XElement ToXml(this JToken jToken, string nodeName = null, string arrayItemName = null)
        {
            return XElement.Parse(ToXmlString(jToken, nodeName, arrayItemName));
        }

        /// <summary>
        /// To the XML string.
        /// </summary>
        /// <param name="jToken">The JToken.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="arrayItemName">Name of the array item.</param>
        /// <returns>System.String.</returns>
        [Obsolete("Use JsonXmlizer.XmlizeToString instead.", true)]
        public static string ToXmlString(this JToken jToken, string nodeName = null, string arrayItemName = null)
        {
            StringBuilder builder = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(builder, new XmlWriterSettings { OmitXmlDeclaration = true }))
            {
                FillXml(writer, jToken, nodeName, arrayItemName);
            }

            return builder.ToString();
        }

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="jToken">The JToken.</param>
        /// <param name="nodeName">Name of the node.</param>
        /// <param name="arrayItemName">Name of the array item.</param>
        /// <returns>XElement.</returns>
        private static void FillXml(XmlWriter writer, JToken jToken, string nodeName = null, string arrayItemName = null)
        {
            if (jToken != null)
            {
                //Node start
                writer.WriteStartElement(nodeName.SafeToString(nodeRoot));
                writer.WriteStartAttribute(attributeType);
                writer.WriteValue(jToken.Type.ToString());
                writer.WriteEndAttribute();

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
                            // Force to prepend nodeNamePrefix to avoid numeric node started.
                            var name = nodeNamePrefix + one.Name;
                            FillXml(writer, one.Value, name);
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
        /// To the JToken
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        [Obsolete("Use JsonXmlizer.Dexmlize instead.", true)]
        public static JToken ToJToken(this XElement xml)
        {
            return InternalToJToken(xml);
        }

        /// <summary>
        /// To the JToken.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>JToken.</returns>
        internal static JToken InternalToJToken(this XElement xml)
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
                                array.AddIfNotNull(InternalToJToken(one));
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
                                // Keep compatibility for those which not use nodeNamePrefix.
                                var name = one.Name.LocalName.StartsWith(nodeNamePrefix) ? one.Name.LocalName.Substring(nodeNamePrefix.Length) : one.Name.LocalName;
                                jObject.Add(name, InternalToJToken(one));
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
    }

}
