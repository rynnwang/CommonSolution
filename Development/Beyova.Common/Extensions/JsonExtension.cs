using System;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class JsonExtension.
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// The iso date time converter
        /// </summary>
        public static readonly IsoDateTimeConverter IsoDateTimeConverter = new IsoDateTimeConverter
        {
            DateTimeFormat = CommonExtension.fullDateTimeTZFormat,
            DateTimeStyles = DateTimeStyles.AdjustToUniversal,
            Culture = CultureInfo.InvariantCulture
        };

        #region Json

        /// <summary>
        /// To the json.
        /// If <c>converters</c> is not specified, isoDateTimeConverter (DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ" and DateTimeStyles = DateTimeStyles.AdjustToUniversal) would be used by default.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="indentedFormat">The indented format.</param>
        /// <param name="converters">The converters.</param>
        /// <returns>System.String.</returns>
        public static string ToJson(this object anyObject, bool indentedFormat, params JsonConverter[] converters)
        {
            return JsonConvert.SerializeObject(anyObject, indentedFormat ? Formatting.Indented : Formatting.None, converters == null ? IsoDateTimeConverter.AsArray() : converters);
        }

        /// <summary>
        /// To the json.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.String.</returns>
        public static string ToJson(this object anyObject)
        {
            return JsonConvert.SerializeObject(anyObject, Formatting.Indented, IsoDateTimeConverter.AsArray());
        }

        /// <summary>
        /// Safes to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static T SafeToObject<T>(this JToken json)
        {
            return json == null ? default(T) : json.ToObject<T>();
        }

        /// <summary>
        /// Tries the convert json to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString">The json string.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>T.</returns>
        public static T TryConvertJsonToObject<T>(this string jsonString, out Exception exception)
        {
            exception = null;

            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(jsonString);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Tries the convert json to object.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>System.Object.</returns>
        public static object TryConvertJsonToObject(this string jsonString, out Exception exception)
        {
            exception = null;

            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                try
                {
                    return JsonConvert.DeserializeObject(jsonString);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            return null;
        }

        /// <summary>
        /// Tries the convert json to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString">The json string.</param>
        /// <returns>T.</returns>
        public static T TryConvertJsonToObject<T>(this string jsonString)
        {
            Exception exception = null;
            return TryConvertJsonToObject<T>(jsonString, out exception);
        }

        /// <summary>
        /// Tries the convert json to object.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns>System.Object.</returns>
        public static object TryConvertJsonToObject(this string jsonString)
        {
            Exception exception = null;
            return TryConvertJsonToObject(jsonString, out exception);
        }

        #endregion Json

        /// <summary>
        /// Gets the JSON object by specific x-path.
        /// </summary>
        /// <param name="jObject">The JSON object in <see cref="JObject" /> type.</param>
        /// <param name="xPath">The x-path.</param>
        /// <returns>
        /// The matched <see cref="JToken" /> instance. If not found, return null.
        /// </returns>
        /// <example>
        /// Samples below show you how to <c>XPath</c> method and the expected result.
        ///   <code>
        /// string json = @"{Property1: {Array:['item1','item2','item3'],Count:3}, Property2: 'hello'}".Replace('\'', '"');
        /// var obj = JToken.Parse(json);   //Parse JSON object from string.
        /// obj = obj.XPath("Property1/Array"); //obj = {Array:['item1','item2','item3']}
        /// var result = obj.XPath("/Property1/Count"); // obj = {Count:3}
        /// var result2 = obj.XPath("/Property1/Array[2]"); //obj = "item3"
        ///   </code>
        /// Note that, if the xPath starts "/", it means to be from the root node, otherwise, from current node.
        ///   </example>
        private static JToken GetJTokenByXPath(this JToken jObject, string xPath)
        {
            JToken result = null;

            if (jObject != null && !string.IsNullOrWhiteSpace(xPath))
            {
                xPath = xPath.Trim();
                var index = xPath.IndexOf('/');

                if (index < 0)
                {
                    result = jObject.SelectToken(xPath);
                }
                else if (index == 0)
                {
                    result = GetJTokenByXPath(jObject.Root, xPath.Substring(1));
                }
                else if (index == (xPath.Length - 1))
                {
                    result = GetJTokenByXPath(jObject, xPath.Substring(0, index));
                }
                else
                {
                    result = GetJTokenByXPath(jObject.SelectToken(xPath.Substring(0, index)), xPath.Substring(index + 1));
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the JSON object by specific x-path.
        /// </summary>
        /// <param name="jObject">The JSON object in <see cref="JObject" /> type.</param>
        /// <param name="xPath">The x-path.</param>
        /// <returns>
        /// The matched <see cref="JToken" /> instance. If not found, return null.
        /// </returns>
        /// <example>
        /// Samples below show you how to <c>XPath</c> method and the expected result.
        ///   <code>
        /// string json = @"{Property1: {Array:['item1','item2','item3'],Count:3}, Property2: 'hello'}".Replace('\'', '"');
        /// var obj = JToken.Parse(json);   //Parse JSON object from string.
        /// obj = obj.XPath("Property1/Array"); //obj = {Array:['item1','item2','item3']}
        /// var result = obj.XPath("/Property1/Count"); // obj = {Count:3}
        /// var result2 = obj.XPath("/Property1/Array[2]"); //obj = "item3"
        ///   </code>
        /// Note that, if the xPath starts "/", it means to be from the root node, otherwise, from current node.
        ///   </example>
        public static JToken XPath(this JObject jObject, string xPath)
        {
            return GetJTokenByXPath(jObject, xPath);
        }

        /// <summary>
        /// Gets the JSON object by specific x-path.
        /// </summary>
        /// <param name="jObject">The JSON object in <see cref="JToken" /> type.</param>
        /// <param name="xPath">The x-path.</param>
        /// <returns>
        /// The matched <see cref="JToken" /> instance. If not found, return null.
        /// </returns>
        /// <example>
        /// Samples below show you how to <c>XPath</c> method and the expected result.
        ///   <code>
        /// string json = @"{Property1: {Array:['item1','item2','item3'],Count:3}, Property2: 'hello'}".Replace('\'', '"');
        /// var obj = JToken.Parse(json);   //Parse JSON object from string.
        /// obj = obj.XPath("Property1/Array"); //obj = {Array:['item1','item2','item3']}
        /// var result = obj.XPath("/Property1/Count"); // obj = {Count:3}
        /// var result2 = obj.XPath("/Property1/Array[2]"); //obj = "item3"
        ///   </code>
        /// Note that, if the xPath starts "/", it means to be from the root node, otherwise, from current node.
        ///   </example>
        public static JToken XPath(this JToken jObject, string xPath)
        {
            return GetJTokenByXPath(jObject, xPath);
        }

        /// <summary>
        /// Tries to get value.
        /// If jObject is null, return default value of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jObject">The j object.</param>
        /// <returns>T.</returns>
        public static T TryGetValue<T>(this JToken jObject)
        {
            return jObject == null ? default(T) : jObject.Value<T>();
        }

        /// <summary>
        /// Tries to deserialize object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>T.</returns>
        public static T TryDeserializeAsObject<T>(this string value)
        {
            try
            {
                value.CheckEmptyString("value");
                return JsonConvert.DeserializeObject<T>(value, IsoDateTimeConverter);
            }
            catch { }

            return default(T);
        }

        /// <summary>
        /// Tries the parse to JTOKEN.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns>JToken.</returns>
        public static JToken TryParseToJToken(this string jsonString)
        {
            if (!string.IsNullOrWhiteSpace(jsonString))
            {
                try
                {
                    return JToken.Parse(jsonString);
                }
                catch { }
            }

            return null;
        }

        /// <summary>
        /// Parses to j token.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns>JToken.</returns>
        public static JToken ParseToJToken(this string jsonString)
        {
            return (!string.IsNullOrWhiteSpace(jsonString)) ? JToken.Parse(jsonString) : null;
        }

        /// <summary>
        /// Parses to j object.
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns></returns>
        public static JObject ParseToJObject(this string jsonString)
        {
            return (!string.IsNullOrWhiteSpace(jsonString)) ? JObject.Parse(jsonString) : null;
        }

        /// <summary>
        /// Objects to json token.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// JToken.
        /// </returns>
        public static JToken ObjectToJToken(this object obj)
        {
            return obj.SafeToString().ParseToJToken();
        }

        /// <summary>
        /// Objects to json object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static JObject ObjectToJObject(this object obj)
        {
            return obj.SafeToString().ParseToJObject();
        }

        /// <summary>
        /// Finds the object.
        /// </summary>
        /// <param name="jObject">The j object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>JToken.</returns>
        public static JToken GetProperty(this JObject jObject, string propertyName, bool ignoreCase = true)
        {
            if (jObject != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                return (from one in jObject.Properties() where string.Equals(one.Name, propertyName, ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture) select one.Value).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Safes the get value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jObject">The j object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        public static T SafeGetValue<T>(this JObject jObject, string propertyName, bool ignoreCase = false, T defaultValue = default(T))
        {
            if (jObject != null && !string.IsNullOrWhiteSpace(propertyName))
            {
                var property = jObject.GetProperty(propertyName, ignoreCase);
                if (property != null)
                {
                    return property.Value<T>();
                }
            }

            return defaultValue;
        }
    }
}