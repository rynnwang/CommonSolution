using Newtonsoft.Json.Linq;
using System;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Collections;

namespace ifunction
{
    /// <summary>
    /// Class StrickJsonSerializer.
    /// </summary>
    public static class StrickJsonSerializer
    {
        /// <summary>
        /// The null j token
        /// </summary>
        public static readonly JToken NullJToken = JToken.Parse("null");

        /// <summary>
        /// The undefined j token
        /// </summary>
        public static readonly JToken UndefinedJToken = JToken.Parse("undefined");

        /// <summary>
        /// To the XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <param name="indentedFormat">if set to <c>true</c> [indented format].</param>
        /// <param name="ignoreNullFields">if set to <c>true</c> [ignore null fields].</param>
        /// <returns>XElement.</returns>
        public static string ToStickJson<T>(this object anyObject, bool indentedFormat = true, bool ignoreNullFields = false)
        {
            StringBuilder builder = new StringBuilder();
            JsonWriter writer = new JsonTextWriter(new StringWriter(builder))
            {
                Formatting = indentedFormat ? Formatting.Indented : Formatting.None
            };

            WriteData(writer, anyObject, typeof(T), ignoreNullFields);

            return builder.ToString();
        }

        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="anyObject">Any object.</param>
        /// <param name="type">The type.</param>
        /// <param name="ignoreNullFields">if set to <c>true</c> [ignore null fields].</param>
        private static void WriteData(JsonWriter writer, object anyObject, Type type, bool ignoreNullFields)
        {
            if (anyObject == null)
            {
                writer.WriteNull();
            }
            else
            {
                if (type.IsClass)
                {
                    var jToken = anyObject as JToken;

                    if (jToken != null)
                    {
                        jToken.WriteTo(writer);
                    }
                    else if (type.IsDictionary())
                    {
                        writer.WriteStartObject();

                        var dictionary = anyObject as IDictionary;
                        var genericType = type.GenericTypeArguments.FirstOrDefault() ?? typeof(object);

                        foreach (var key in dictionary.Keys)
                        {
                            writer.WritePropertyName(key.ToString());
                            WriteData(writer, dictionary[key], genericType, ignoreNullFields);
                        }

                        writer.WriteEndObject();
                    }
                    else if (type.IsCollection())
                    {
                        writer.WriteStartArray();
                        var genericType = type.GenericTypeArguments.FirstOrDefault() ?? typeof(object);
                        foreach (var one in anyObject as ICollection)
                        {
                            WriteData(writer, one, genericType, ignoreNullFields);
                        }

                        writer.WriteEndArray();
                    }
                    else if (type == typeof(string))
                    {
                        writer.WriteValue((string)anyObject);
                    }
                    else
                    {
                        writer.WriteStartObject();

                        foreach (var one in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                        {
                            var value = one.GetValue(anyObject);

                            if (value == null && ignoreNullFields)
                            {
                                continue;
                            }
                            writer.WritePropertyName(one.Name);
                            WriteData(writer, value, one.PropertyType, ignoreNullFields);
                        }

                        foreach (var one in type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                        {
                            var value = one.GetValue(anyObject);

                            if (value == null && ignoreNullFields)
                            {
                                continue;
                            }
                            writer.WritePropertyName(one.Name);
                            WriteData(writer, value, one.FieldType, ignoreNullFields);
                        }

                        writer.WriteEndObject();
                    }
                }
                else if (type.IsValueType)
                {
                    if (type.IsEnum)
                    {
                        writer.WriteValue((int?)anyObject);
                    }
                    else
                    {
                        writer.WriteValue(anyObject);
                    }
                }
            }
        }
    }

}
