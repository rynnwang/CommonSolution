using System;

namespace Beyova
{
    /// <summary>
    /// Class GeographyTier.
    /// </summary>
    public class GeographyTier : BaseObject, IGlobalObjectName
    {
        #region Properties

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

        /// <summary>
        /// Gets or sets the tier value.
        /// </summary>
        /// <value>
        /// The tier value.
        /// </value>
        public GeographyTierLevel TierValue
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the post code.
        /// </summary>
        /// <value>
        /// The post code.
        /// </value>
        public string PostCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the country key.
        /// </summary>
        /// <value>
        /// The country key.
        /// </value>
        public Guid CountryKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent key.
        /// </summary>
        /// <value>
        /// The parent key.
        /// </value>
        public Guid? ParentKey
        {
            get;
            set;
        }

        #endregion
    }
}
