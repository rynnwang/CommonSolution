using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Beyova
{
    /// <summary>
    /// Class SafeContractResolver.
    /// </summary>
    internal class SafeContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// The HTTP request ignore properties
        /// </summary>
        protected static string[] httpRequestIgnoreProperties = new string[] {
        "HttpContextBase","HttpContext","ServerVariables","Browser","HtmlTextWriter","ClrVersion","TagWriter","EcmaScriptVersion","MSDomVersion","W3CDomVersion","Unvalidated","Params"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeContractResolver" /> class.
        /// </summary>
        public SafeContractResolver()
            : base()
        {
        }

        /// <summary>
        /// Creates properties for the given 
        /// <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.</returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> result = base.CreateProperties(type, memberSerialization);

            if (type == typeof(HttpContext) || type == typeof(HttpListenerContext) || type == typeof(HttpContextBase))
            {
                var x = from property in result where property.PropertyName == "Request" select property;
                result = x.ToList();
            }
            else if (type == typeof(HttpResponse) || type == typeof(HttpListenerResponse) || type == typeof(HttpResponseBase))
            {
                var x = from property in result where property.PropertyName != "Length" select property;
                result = x.ToList();
            }
            else if (type == typeof(HttpRequest) || type == typeof(HttpListenerRequest) || type == typeof(HttpRequestBase))
            {
                var x = from property in result where !httpRequestIgnoreProperties.Contains(property.PropertyName, true) select property;
                result = x.ToList();
            }
            else if (typeof(Stream).IsAssignableFrom(type) || typeof(System.Web.Profile.DefaultProfile) == type)
            {
                result = new List<JsonProperty>();
            }

            //else if (type == typeof(HttpRequest) || type == typeof(HttpRequestBase) || type == typeof(HttpListenerRequest))
            //{

            //}

            return result;
        }
    }
}
