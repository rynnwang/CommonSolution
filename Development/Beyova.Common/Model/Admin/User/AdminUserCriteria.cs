using System;

namespace Beyova
{
    /// <summary>
    /// Class AdminUserCriteria.
    /// </summary>
    public class AdminUserCriteria : AdminUserInfo, ICriteria
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the role key.
        /// </summary>
        /// <value>The role key.</value>
        public Guid? RoleKey { get; set; }
    }
}