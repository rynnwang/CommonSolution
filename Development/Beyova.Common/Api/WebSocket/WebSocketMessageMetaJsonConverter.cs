//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Beyova.ExceptionSystem;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace Beyova.Api.WebSockets
//{
//    /// <summary>
//    /// Class WebSocketMessageMetaJsonConverter.
//    /// </summary>
//    public class WebSocketMessageMetaJsonConverter : JsonConverter
//    {
//        /// <summary>
//        /// Gets or sets the message action alias.
//        /// </summary>
//        /// <value>The message action alias.</value>
//        public string MessageActionAlias { get; protected set; }

//        /// <summary>
//        /// Gets or sets the message body alias.
//        /// </summary>
//        /// <value>The message body alias.</value>
//        public string MessageBodyAlias { get; protected set; }

//        /// <summary>
//        /// Gets or sets the message exception alias.
//        /// </summary>
//        /// <value>The message exception alias.</value>
//        public string MessageExceptionAlias { get; protected set; }

//        /// <summary>
//        /// Gets or sets the message referrer alias.
//        /// </summary>
//        /// <value>The message referrer alias.</value>
//        public string MessageReferrerAlias { get; protected set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WebSocketMessageMetaJsonConverter"/> class.
//        /// </summary>
//        /// <param name="settings">The settings.</param>
//        public WebSocketMessageMetaJsonConverter(WebSocketSettings settings)
//        {
//            if (settings != null)
//            {
//                this.MessageActionAlias = settings.MessageActionAlias.SafeToString("action");
//                this.MessageBodyAlias = settings.MessageBodyAlias.SafeToString("body");
//                this.MessageExceptionAlias = settings.MessageExceptionAlias.SafeToString("exception");
//                this.MessageReferrerAlias = settings.MessageReferrerAlias.SafeToString("referrer");
//            }
//        }

//        /// <summary>
//        /// Determines whether this instance can convert the specified object type.
//        /// </summary>
//        /// <param name="objectType">Type of the object.</param>
//        /// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
//        public override bool CanConvert(Type objectType)
//        {
//            return objectType.IsAssignableFrom(typeof(WebSocketMessageMeta));
//        }

//        /// <summary>
//        /// Reads the JSON representation of the object.
//        /// </summary>
//        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
//        /// <param name="objectType">Type of the object.</param>
//        /// <param name="existingValue">The existing value of object being read.</param>
//        /// <param name="serializer">The calling serializer.</param>
//        /// <returns>The object value.</returns>
//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            JObject jsonObject = JObject.Load(reader);

//            if (jsonObject == null)
//            {
//                return null;
//            }

//            var result = new WebSocketMessageMeta();

//            foreach (var one in jsonObject.Properties())
//            {
//                if (one.Name.Equals(this.MessageBodyAlias))
//                {
//                    result.Body = one.Value;
//                }
//                else if (one.Name.Equals(this.MessageActionAlias))
//                {
//                    result.Action = one.Value.ToObject<string>();
//                }
//                else if (one.Name.Equals(this.MessageExceptionAlias))
//                {
//                    result.Exception = one.Value.ToObject<ExceptionInfo>();
//                }
//                else if (one.Name.Equals(this.MessageReferrerAlias))
//                {
//                    result.MessageReferrer = one.Value.ToObject<string>();
//                }
//            }

//            return result;
//        }

//        /// <summary>
//        /// Writes the JSON representation of the object.
//        /// </summary>
//        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
//        /// <param name="value">The value.</param>
//        /// <param name="serializer">The calling serializer.</param>
//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            var message = value as WebSocketMessageMeta;
//            writer.WriteStartObject();
//            writer.WritePropertyName(this.MessageActionAlias);
//            serializer.Serialize(writer, message.Action);

//            writer.WritePropertyName(this.MessageBodyAlias);
//            serializer.Serialize(writer, message.Body);

//            writer.WritePropertyName(this.MessageExceptionAlias);
//            serializer.Serialize(writer, message.Exception);

//            writer.WritePropertyName(this.MessageReferrerAlias);
//            serializer.Serialize(writer, message.MessageReferrer);

//            writer.WriteEndObject();
//        }
//    }
//}