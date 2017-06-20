using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class EnumStringConverter.
    /// </summary>
    public class EnumStringConverter : JsonConverter
    {
        /// <summary>
        /// Enum EnumStringNamingRule
        /// </summary>
        public enum EnumStringNamingRule
        {
            /// <summary>
            /// The default
            /// </summary>
            Default = 0,

            /// <summary>
            /// The lower camel case
            /// </summary>
            LowerCamelCase = 1,

            /// <summary>
            /// The by slash
            /// </summary>
            BySlash = 2,

            /// <summary>
            /// The lower cases
            /// </summary>
            LowerCases = 3
        }

        /// <summary>
        /// Gets or sets the naming rule.
        /// </summary>
        /// <value>
        /// The naming rule.
        /// </value>
        public EnumStringNamingRule NamingRule { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumStringConverter" /> class.
        /// </summary>
        /// <param name="namingRule">The naming rule.</param>
        public EnumStringConverter(EnumStringNamingRule namingRule = EnumStringNamingRule.Default)
            : base()
        {
            this.NamingRule = namingRule;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise,
        /// <c>false</c>.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum || (objectType.IsNullable() && Nullable.GetUnderlyingType(objectType).IsEnum);
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
            var text = reader.Value.SafeToString();
            object enumValue = null;

            return EnumExtension.TryParseEnum(
                objectType,
                (NamingRule == EnumStringNamingRule.BySlash ? text.Replace("-", string.Empty) : text),
                out enumValue,
                true) ? enumValue : EnumExtension.GetEnumDefaultValue(objectType);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var text = value.SafeToString();

            if (!string.IsNullOrWhiteSpace(text))
            {
                switch (this.NamingRule)
                {
                    case EnumStringNamingRule.BySlash:
                        text = StringRegexExtension.SplitByUpperCases(text, "-");
                        break;

                    case EnumStringNamingRule.LowerCamelCase:
                        text = Convert.ToString(Char.ToLowerInvariant(text[0])) + (text.Length > 1 ? text.Substring(1) : string.Empty);
                        break;

                    case EnumStringNamingRule.LowerCases:
                        text = text.ToLowerInvariant();
                        break;
                }
            }

            writer.WriteValue(string.IsNullOrWhiteSpace(text) ? null : text);
        }
    }
}