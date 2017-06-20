//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Beyova.Api.WebSockets
//{
//    /// <summary>
//    /// Class WebSocketHub.
//    /// </summary>
//    public class WebSocketHub : WebSocketHub<WebSocketConnection>
//    {
//    }

//    /// <summary>
//    /// Class WebSocketHub.
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    public class WebSocketHub<T> where T : WebSocketConnection
//    {
//        /// <summary>
//        /// The hubs
//        /// </summary>
//        internal protected static Dictionary<Type, WebSocketHub<T>> hubs = new Dictionary<Type, WebSocketHub<T>>();

//        /// <summary>
//        /// The locker
//        /// </summary>
//        internal protected static object locker = new object();

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WebSocketHub{T}"/> class.
//        /// </summary>
//        internal protected WebSocketHub()
//        {
//        }

//        /// <summary>
//        /// Gets the instance.
//        /// </summary>
//        /// <value>The instance.</value>
//        public static WebSocketHub<T> Instance
//        {
//            get
//            {
//                WebSocketHub<T> result = null;
//                if (!hubs.TryGetValue(typeof(T), out result))
//                {
//                    lock (locker)
//                    {
//                        if (!hubs.TryGetValue(typeof(T), out result))
//                        {
//                            result = new WebSocketHub<T>();
//                            hubs.Add(typeof(T), result);
//                        }
//                    }
//                }

//                return result as WebSocketHub<T>;
//            }
//        }
//    }
//}