using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityContext.
    /// </summary>
    public static class GravityContext
    {
        [ThreadStatic]
        private static ProductInfo _currentProductInfo = null;

        /// <summary>
        /// The product key
        /// </summary>
        [ThreadStatic]
        private static Guid? _productKey;

        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        /// <value>The product key.</value>
        public static Guid? ProductKey
        {
            get { return _productKey; }
            internal set { _productKey = value; }
        }

        /// <summary>
        /// Gets the product information.
        /// </summary>
        /// <value>The product information.</value>
        public static ProductInfo ProductInfo
        {
            get { return _currentProductInfo; }
            internal set { _currentProductInfo = value; }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        internal static void Dispose()
        {
            _productKey = null;
            _currentProductInfo = null;
        }
    }
}