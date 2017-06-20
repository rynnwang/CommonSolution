//using System;

//namespace Beyova.Api.WebSockets
//{
//    /// <summary>
//    /// Class ApiContractAttribute. It is used to define interface which used as Web-Socket API.
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
//    public class WebSocketApiContractAttribute : Attribute
//    {
//        /// <summary>
//        /// Gets or sets the name.
//        /// </summary>
//        /// <value>The name.</value>
//        public string Name { get; protected set; }

//        /// <summary>
//        /// Gets or sets a value indicating whether [token required].
//        /// </summary>
//        /// <value><c>true</c> if [token required]; otherwise, <c>false</c>.</value>
//        public bool TokenRequired { get; protected set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="WebSocketApiContractAttribute" /> class.
//        /// </summary>
//        /// <param name="name">The name. This name would be used as destination for web-socket connection establish.</param>
//        /// <param name="tokenRequired">if set to <c>true</c> [token required].</param>
//        public WebSocketApiContractAttribute(string name, bool tokenRequired = true)
//        {
//            this.Name = name;
//        }
//    }
//}