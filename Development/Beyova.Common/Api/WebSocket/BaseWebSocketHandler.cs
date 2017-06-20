//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Beyova.Api.WebSockets
//{
//    /// <summary>
//    /// Class BaseWebSocketHandler.
//    /// </summary>
//    public abstract class BaseWebSocketHandler
//    {
//        /// <summary>
//        /// The _settings
//        /// </summary>
//        protected WebSocketSettings _settings;

//        /// <summary>
//        /// The _message json converter
//        /// </summary>
//        protected WebSocketMessageMetaJsonConverter _messageJsonConverter;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="BaseWebSocketHandler"/> class.
//        /// </summary>
//        /// <param name="settings">The settings.</param>
//        protected BaseWebSocketHandler(WebSocketSettings settings)
//        {
//            this._settings = settings;
//            this._messageJsonConverter = new WebSocketMessageMetaJsonConverter(settings);
//        }

//        /// <summary>
//        /// Serializes the message.
//        /// </summary>
//        /// <param name="message">The message.</param>
//        /// <returns>System.String.</returns>
//        protected string SerializeMessage(WebSocketMessageMeta message)
//        {
//            return message == null ? string.Empty : message.ToJson(true, this._messageJsonConverter);
//        }
//    }
//}