using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminUserCriteria.
    /// </summary>
    public class AdminUserCriteria : AdminUserInfoBase, ICriteria
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get; set;
        }
    }
}
