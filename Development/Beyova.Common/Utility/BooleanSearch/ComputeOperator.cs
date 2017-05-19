using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Enum ComputeOperator
    /// </summary>
    public enum ComputeOperator
    {
        /// <summary>
        /// Value indicating it is equals
        /// </summary>
        Equals = 0,
        /// <summary>
        /// Value indicating it is not equals
        /// </summary>
        NotEquals = 1,
        /// <summary>
        /// Value indicating it is great than or equals
        /// </summary>
        GreatThanOrEquals,
        /// <summary>
        /// Value indicating it is less than or equals
        /// </summary>
        LessThanOrEquals,
        /// <summary>
        /// Value indicating it is great than
        /// </summary>
        GreatThan,
        /// <summary>
        /// Value indicating it is less than
        /// </summary>
        LessThan,
        /// <summary>
        /// Value indicating it is start with
        /// </summary>
        StartWith,
        /// <summary>
        /// Value indicating it is end with
        /// </summary>
        EndWith,
        /// <summary>
        /// Value indicating it is contains
        /// </summary>
        Contains,
        /// <summary>
        /// Value indicating it is contains
        /// </summary>
        NotContains,
        /// <summary>
        /// Value indicating it is exists
        /// </summary>
        Exists
    }
}
