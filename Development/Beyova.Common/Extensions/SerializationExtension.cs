using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Extension class for mail.
    /// </summary>
    public static class SerializationExtension
    {
        /// <summary>
        /// The formatter
        /// </summary>
        private static readonly BinaryFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// The safe json serialization settings
        /// </summary>
        public static readonly JsonSerializerSettings SafeJsonSerializationSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new SafeContractResolver()
        };

        /// <summary>
        /// Serializes to stream.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>MemoryStream.</returns>
        /// <exception cref="OperationFailureException">SerializeToStream</exception>
        public static MemoryStream SerializeToStream(this object objectToSerialize)
        {
            try
            {
                objectToSerialize.CheckNullObject("objectToSerialize");

                using (var memoryStream = new MemoryStream())
                {
                    formatter.Serialize(memoryStream, objectToSerialize);
                    return memoryStream;
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("SerializeToStream", objectToSerialize);
            }
        }

        /// <summary>
        /// Serializes to byte array.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] SerializeToByteArray(this object objectToSerialize)
        {
            return SerializeToStream(objectToSerialize).ToArray();
        }

        /// <summary>
        /// Serializes to base64 string.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="OperationFailureException">SerializeToBase64String</exception>
        public static string SerializeToBase64String(this object objectToSerialize)
        {
            try
            {
                byte[] byteArray = SerializeToByteArray(objectToSerialize);
                return Convert.ToBase64String(byteArray, 0, byteArray.Length);
            }
            catch (Exception ex)
            {
                throw ex.Handle("SerializeToBase64String", objectToSerialize);
            }
        }

        /// <summary>
        /// Serializes to XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="omitXmlDeclaration">if set to <c>true</c> [omit XML declaration].</param>
        /// <param name="indentedXml">if set to <c>true</c> [indented XML].</param>
        /// <returns>System.String.</returns>
        public static XElement SerializeToXml<T>(this T objectToSerialize, Encoding encoding = null, bool omitXmlDeclaration = true, bool indentedXml = false)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            try
            {
                objectToSerialize.CheckNullObject("objectToSerialize");

                var serializer = new XmlSerializer(typeof(T));
                using (var stream = new MemoryStream())
                {
                    using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Encoding = encoding, OmitXmlDeclaration = omitXmlDeclaration, Indent = indentedXml }))
                    {
                        serializer.Serialize(writer, objectToSerialize);
                        return XElement.Parse(encoding.GetString(stream.ToArray()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("SerializeToXml", new { Encoding = encoding.EncodingName, OmitXmlDeclaration = omitXmlDeclaration, IndentedXml = indentedXml });
            }
        }

        /// <summary>
        /// Deserializes to object.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="closeStream">if set to <c>true</c> [close stream].</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="OperationFailureException">DeserializeToObject</exception>
        public static object DeserializeToObject(this MemoryStream stream, bool closeStream = true)
        {
            try
            {
                stream.CheckNullObject("stream");
                return formatter.Deserialize(stream);
            }
            catch (Exception ex)
            {
                throw ex.Handle("DeserializeToObject");
            }
            finally
            {
                if (stream != null && closeStream)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Deserializes from base64 string to object.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <returns>System.Object.</returns>
        public static object DeserializeFromBase64StringToObject(this string base64String)
        {
            try
            {
                base64String.CheckEmptyString("base64String");

                var byteArray = Convert.FromBase64String(base64String);
                return byteArray.DeserializeToObject();
            }
            catch (Exception ex)
            {
                throw ex.Handle("DeserializeFromBase64StringToObject", base64String);
            }
        }

        /// <summary>
        /// Deserializes to object.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="OperationFailureException">DeserializeToObject</exception>
        public static object DeserializeToObject(this byte[] byteArray)
        {
            try
            {
                byteArray.CheckNullObject("byteArray");
                using (var memoryStream = new MemoryStream(byteArray, 0, byteArray.Length))
                {
                    return memoryStream.DeserializeToObject();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("DeserializeToObject");
            }
        }

        /// <summary>
        /// Deserializes to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns>T.</returns>
        public static T DeserializeToObject<T>(this MemoryStream stream)
        {
            var obj = stream.DeserializeToObject(true);

            if (obj != null)
            {
                return (T)obj;
            }

            return default(T);
        }

        /// <summary>
        /// Deserializes to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <returns>T.</returns>
        public static T DeserializeToObject<T>(this byte[] bytes)
        {
            var obj = bytes.DeserializeToObject();

            if (obj != null)
            {
                return (T)obj;
            }

            return default(T);
        }

        /// <summary>
        /// Deserailizes to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="base64String">The base64 string.</param>
        /// <returns>T.</returns>
        public static T DeserializeToObject<T>(string base64String)
        {
            if (!string.IsNullOrWhiteSpace(base64String))
            {
                byte[] byteArray = Convert.FromBase64String(base64String);
                return byteArray.DeserializeToObject<T>();
            }

            return default(T);
        }

        /// <summary>
        /// Deserializes to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>T.</returns>
        public static T DeserializeXmlToObject<T>(this string xml, Encoding encoding = null)
        {
            if (!string.IsNullOrWhiteSpace(xml))
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(T));
                    using (var memoryStream = new StreamReader(new MemoryStream((encoding ?? Encoding.UTF8).GetBytes(xml))))
                    {
                        return (T)serializer.Deserialize(memoryStream);
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle("DeserializeXmlToObject", xml);
                }
            }

            return default(T);
        }

        /// <summary>
        /// Deserializes to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>T.</returns>
        public static T DeserializeXmlToObject<T>(this XElement xml, Encoding encoding = null)
        {
            if (xml != null)
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(xml.CreateReader());
                }
                catch (Exception ex)
                {
                    throw ex.Handle("DeserializeXmlToObject", xml?.ToString());
                }
            }

            return default(T);
        }

        #region Serialization

        /// <summary>
        /// To the pure XML.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="createDeclaration">if set to <c>true</c> [create declaration].</param>
        /// <returns>XElement.</returns>
        public static XElement ToPureXml(this object anyObject, bool createDeclaration = false)
        {
            if (anyObject != null)
            {
                var stringBuilder = new StringBuilder();

                var ns = new XmlSerializerNamespaces();
                //Add an empty namespace and empty value
                ns.Add(string.Empty, string.Empty);
                var settings = new XmlWriterSettings { OmitXmlDeclaration = !createDeclaration };

                // Remove the <?xml version="1.0" encoding="utf-8"?>

                var writer = XmlWriter.Create(stringBuilder, settings);
                //Create the serializer
                var serializer = new XmlSerializer(anyObject.GetType());

                //Serialize the object with our own namespaces (notice the overload)
                serializer.Serialize(writer, anyObject, ns);
                writer.Flush();

                return XElement.Parse(stringBuilder.ToString());
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts from pure XML to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlObject">The XML object.</param>
        /// <returns>T.</returns>
        public static T PureXmlToObject<T>(this XElement xmlObject)
        {
            var reader = xmlObject.CreateReader();

            var serializer = new XmlSerializer(typeof(T));
            var obj = serializer.Deserialize(reader);

            if (obj != null)
            {
                return (T)obj;
            }
            else
            {
                return default(T);
            }
        }

        #endregion
    }
}
