using System;
using System.Collections.Generic;
using System.Reflection;

namespace ifunction.RestApi
{
    /// <summary>
    /// Class RuntimeRoute.
    /// </summary>
    public class RuntimeRoute
    {
        /// <summary>
        /// Gets or sets the transport.
        /// </summary>
        /// <value>The transport.</value>
        public ApiTransportAttribute Transport { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is transport.
        /// </summary>
        /// <value><c>true</c> if this instance is transport; otherwise, <c>false</c>.</value>
        public bool IsTransport
        {
            get { return Transport != null; }
        }

        /// <summary>
        /// Gets or sets the method information.
        /// </summary>
        /// <value>The method information.</value>
        public MethodInfo MethodInfo { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the instance.
        /// </summary>
        /// <value>The type of the instance.</value>
        public Type InstanceType { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is token required.
        /// </summary>
        /// <value><c>true</c> if this instance is token required; otherwise, <c>false</c>.</value>
        public bool IsTokenRequired { get; protected set; }

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public object Instance { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is action used.
        /// </summary>
        /// <value><c>true</c> if this instance is action used; otherwise, <c>false</c>.</value>
        public bool IsActionUsed { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; protected set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public IDictionary<string, ApiPermission> Permissions { get; protected set; }

        /// <summary>
        /// Gets or sets the setting.
        /// </summary>
        /// <value>The setting.</value>
        public RestApiSettings Setting { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeRoute" /> class.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="instanceType">Type of the instance.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="isActionUsed">if set to <c>true</c> [is action used].</param>
        /// <param name="isTokenRequired">if set to <c>true</c> [is token required].</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="setting">The setting.</param>
        /// <param name="permissions">The permissions.</param>
        public RuntimeRoute(MethodInfo methodInfo, Type instanceType, object instance, bool isActionUsed, bool isTokenRequired, string moduleName, string serviceName, RestApiSettings setting, IDictionary<string, ApiPermission> permissions = null)
        {
            this.MethodInfo = methodInfo;
            this.Instance = instance;
            this.IsActionUsed = isActionUsed;
            this.ServiceName = serviceName;
            this.InstanceType = instanceType;
            this.ModuleName = moduleName;
            this.IsTokenRequired = isTokenRequired;
            this.Setting = setting;

            Permissions = permissions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeRoute"/> class.
        /// </summary>
        /// <param name="transport">The transport.</param>
        public RuntimeRoute(ApiTransportAttribute transport)
        {
            this.Transport = transport;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.MethodInfo == null ? (InstanceType == null ? string.Empty : InstanceType.FullName) : MethodInfo.GetFullName();
        }
    }
}
