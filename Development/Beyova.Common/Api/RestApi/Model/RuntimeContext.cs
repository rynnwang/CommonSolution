﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Beyova.Api;
using Beyova.Cache;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class RuntimeContext.
    /// </summary>
    public class RuntimeContext
    {
        /// <summary>
        /// Gets or sets the API method.
        /// </summary>
        /// <value>The API method.</value>
        public MethodInfo ApiMethod { get; set; }

        /// <summary>
        /// Gets or sets the API instance.
        /// </summary>
        /// <value>The API instance.</value>
        public object ApiInstance { get; set; }

        /// <summary>
        /// Gets or sets the name of the API service.
        /// </summary>
        /// <value>The name of the API service.</value>
        public string ApiServiceName { get; set; }

        /// <summary>
        /// Gets or sets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        public string ResourceName { get; set; }

        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <value>The realm.</value>
        public string Realm { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the parameter1.
        /// </summary>
        /// <value>The parameter1.</value>
        public string Parameter1 { get; set; }

        /// <summary>
        /// Gets or sets the parameter2.
        /// </summary>
        /// <value>The parameter2.</value>
        public string Parameter2 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is action used.
        /// </summary>
        /// <value><c>true</c> if this instance is action used; otherwise, <c>false</c>.</value>
        public bool IsActionUsed { get; set; }

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        /// <value>The settings.</value>
        public RestApiSettings Settings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is void.
        /// </summary>
        /// <value><c>true</c> if this instance is void; otherwise, <c>false</c>.</value>
        public bool? IsVoid { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets the entity key.
        /// </summary>
        /// <value>The entity key.</value>
        public string EntityKey
        {
            get
            {
                return IsActionUsed ? Parameter2 : Parameter1;
            }
        }

        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        /// <value>The name of the action.</value>
        public string ActionName
        {
            get
            {
                return IsActionUsed ? Parameter1 : null;
            }
        }

        /// <summary>
        /// Gets or sets the customized header keys.
        /// </summary>
        /// <value>The customized header keys.</value>
        public List<string> CustomizedHeaderKeys { get; set; }

        /// <summary>
        /// Gets the operation parameters.
        /// </summary>
        /// <value>The operation parameters.</value>
        internal RuntimeApiOperationParameters OperationParameters { get; set; }

        #region Cache

        /// <summary>
        /// Gets or sets the API cache status.
        /// </summary>
        /// <value>
        /// The API cache status.
        /// </value>
        internal ApiCacheStatus ApiCacheStatus { get; set; }

        /// <summary>
        /// Gets or sets the API cache identity.
        /// </summary>
        /// <value>
        /// The API cache identity.
        /// </value>
        internal ApiRouteIdentifier ApiCacheIdentity { get; set; }

        /// <summary>
        /// Gets or sets the API cache container.
        /// </summary>
        /// <value>
        /// The API cache container.
        /// </value>
        internal ApiCacheContainer ApiCacheContainer { get; set; }

        /// <summary>
        /// Gets or sets the cached response body.
        /// </summary>
        /// <value>
        /// The cached response body.
        /// </value>
        internal string CachedResponseBody { get; set; }

        #endregion Cache
    }
}