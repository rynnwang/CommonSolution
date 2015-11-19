using System.Collections.Generic;
using ifunction.RestApi;

namespace ifunction
{
    /// <summary>
    /// Interface IFrameworkInitializer
    /// </summary>
    public interface IFrameworkInitializer
    {
        /// <summary>
        /// Gets the API tracking.
        /// </summary>
        /// <value>The API tracking.</value>
        IApiTracking ApiTracking { get; }

        /// <summary>
        /// Gets or sets the configuration reader.
        /// </summary>
        /// <value>The configuration reader.</value>
        IConfigurationReader ConfigurationReader { get; }

        /// <summary>
        /// Gets the current operator thread key.
        /// </summary>
        /// <value>The current operator thread key.</value>
        string CurrentOperatorThreadKey { get; }
    }
}
