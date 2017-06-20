using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class AdaptableEnumConverter. Supports enum and nullable enum.
    /// </summary>
    public class AdaptableEnumConverter<T> : JsonConverter
        where T : struct
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is flag.
        /// </summary>
        /// <value><c>true</c> if this instance is flag; otherwise, <c>false</c>.</value>
        public bool IsFlag { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptableEnumConverter{T}" /> class.
        /// </summary>
        public AdaptableEnumConverter()
            : base()
        {
            var enumType = typeof(T);
            enumType = enumType.IsNullable() ? enumType.GetNullableType() : enumType;
            IsFlag = enumType.IsEnum && enumType.GetCustomAttribute<FlagsAttribute>() != null;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise,
        /// <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsNullable() ? objectType.GetNullableType().IsEnum : objectType.IsEnum;
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Enum.Parse(objectType, reader.Value.SafeToString());
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (IsFlag)
            {
                writer.WriteValue((int)value);
            }
            else
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}