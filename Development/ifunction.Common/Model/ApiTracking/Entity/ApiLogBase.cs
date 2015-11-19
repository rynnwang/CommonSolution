namespace ifunction.ApiTracking.Model
{
    /// <summary>
    /// Class ApiLogBase.
    /// </summary>
    public class ApiLogBase
    {
        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>The service identifier.</value>
        public string ServiceIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the server identifier.
        /// <remarks>Commonly, it is server name.</remarks>
        /// </summary>
        /// <value>The server identifier.</value>
        public string ServerIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public string UserIdentifier { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiLogBase" /> class.
        /// </summary>
        /// <param name="logBase">The log base.</param>
        public ApiLogBase(ApiLogBase logBase)
        {
            if (logBase != null)
            {
                this.ServerIdentifier = logBase.ServerIdentifier;
                this.ServiceIdentifier = logBase.ServiceIdentifier;
                this.UserIdentifier = logBase.UserIdentifier;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiLogBase"/> class.
        /// </summary>
        public ApiLogBase()
        {
        }
    }
}
