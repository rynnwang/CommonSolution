using System;

namespace Beyova
{
    /// <summary>
    /// Class GeographyTierCriteria.
    /// </summary>
    public class GeographyTierCriteria : BaseCriteria, IGlobalObjectName
    {
        #region Properties

        /// <summary>
        /// Gets or sets the post code.
        /// </summary>
        /// <value>The post code.</value>
        public string PostCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent key.
        /// </summary>
        /// <value>The parent key.</value>
        public Guid? ParentKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tier value.
        /// </summary>
        /// <value>The tier value.</value>
        public GeographyTierLevel? TierValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>The culture code.
        /// e.g.: zh-CN, en-US, pt-PT, etc.</value>
        public string CultureCode
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GeographyTierCriteria" /> class.
        /// </summary>
        public GeographyTierCriteria()
            : base()
        {
        }

        #endregion
    }
}
