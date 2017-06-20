using System;

namespace Beyova
{
    /// <summary>
    /// Class BaseCriteria.
    /// </summary>
    public class BaseCriteria : ICriteria
    {
        #region Properties

        /// <summary>
        /// Gets or sets the key of the object.
        /// If this value is assigned, other criteria would be ignored.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [order descending].
        /// </summary>
        /// <value><c>true</c> if [order descending]; otherwise, <c>false</c>.</value>
        public bool OrderDescending { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCriteria" /> class.
        /// </summary>
        public BaseCriteria()
        {
        }

        #endregion Constructor
    }
}