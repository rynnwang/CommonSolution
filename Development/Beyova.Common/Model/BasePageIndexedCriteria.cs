namespace Beyova
{
    /// <summary>
    /// Class BasePageIndexedCriteria.
    /// </summary>
    public class BasePageIndexedCriteria : BaseCriteria
    {
        #region Properties

        /// <summary>
        /// Gets or sets the start index.
        /// It could be with <c>Count</c> to do paging.
        /// </summary>
        /// <value>The start index.</value>

        public int StartIndex { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePageIndexedCriteria" /> class.
        /// </summary>
        public BasePageIndexedCriteria()
            : base()
        {
        }

        #endregion Constructor
    }
}