using System;

namespace Beyova
{
    /// <summary>
    /// Class CountryInfo.
    /// </summary>
    public class CountryInfo : IIdentifier, IGlobalObjectName
    {
        #region Properties

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

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
        /// Gets or sets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public int? TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the tel code.
        /// </summary>
        /// <value>
        /// The tel code.
        /// </value>
        public string TelCode { get; set; }

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

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CountryInfo"/> class.
        /// </summary>
        public CountryInfo()
            : base()
        {
        }

        #endregion Constructor
    }
}