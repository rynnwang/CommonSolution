using System;

namespace ifunction.RestApi
{
    /// <summary>
    /// Class ApiTransportAttribute. This attribute is used when an interface needs to call another REST API within same contract. <see cref="IApiTransportAdapter"/> based <c>ApiTransportAdapter</c> is for specifying the destination and change token.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class ApiTransportAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the API transport adapter.
        /// </summary>
        /// <value>The API transport adapter.</value>
        public IApiTransportAdapter ApiTransportAdapter { get; protected set; }

        /// <summary>
        /// Gets the destination host.
        /// </summary>
        /// <value>The destination host.</value>
        public string DestinationHost
        {
            get { return this.ApiTransportAdapter != null ? this.ApiTransportAdapter.DestinationHost : null; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTransportAttribute" /> class.
        /// </summary>
        /// <param name="typeOfApiTransportAdapter">The type which implement <see cref="IApiTransportAdapter" />.</param>
        public ApiTransportAttribute(Type typeOfApiTransportAdapter = null)
            : base()
        {
            if (typeOfApiTransportAdapter != null)
            {
                this.ApiTransportAdapter = Activator.CreateInstance(typeOfApiTransportAdapter) as IApiTransportAdapter;
            }
        }
    }
}
