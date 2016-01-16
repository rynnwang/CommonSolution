using System;

namespace Beyova
{
    /// <summary>
    /// Class CountryInfo.
    /// </summary>
    public class CountryInfo : BaseObject, IGlobalObjectName
    {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>The currency code.</value>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets the ISO2 code.
        /// </summary>
        /// <value>The ISO2 code.</value>
        public string Iso2Code { get; set; }

        /// <summary>
        /// Gets or sets the ISO3 code.
        /// </summary>
        /// <value>The ISO3 code.</value>
        public string Iso3Code { get; set; }

        /// <summary>
        /// Gets or sets the time zone key.
        /// </summary>
        /// <value>The time zone key.</value>
        public Guid? TimeZoneKey { get; set; }

        /// <summary>
        /// Gets or sets the tel code.
        /// </summary>
        /// <value>
        /// The tel code.
        /// </value>
        public string TelCode { get; set; }

        /// <summary>
        /// Gets or sets the geography key.
        /// </summary>
        /// <value>The geography key.</value>
        public Guid? GeographyKey { get; set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>The culture code.</value>
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>The sequence.</value>
        public int Sequence { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryInfo"/> class.
        /// </summary>
        public CountryInfo()
            : base()
        {
        }

        #endregion
    }
}
