using System;

namespace Beyova
{
    /// <summary>
    /// Class CountryInfoCriteria.
    /// </summary>
    public class CountryInfoCriteria : IGlobalObjectName
    {
        #region Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>The culture code.
        /// e.g.: zh-CN, en-US, pt-PT, etc.</value>
        public string CultureCode { get; set; }

        #endregion Properties
    }
}