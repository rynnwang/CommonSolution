using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ifunction.Common.Model
{
    /// <summary>
    /// Enum FilterOperator
    /// </summary>
    public enum FilterOperator
    {
        /// <summary>
        /// Value indicating it is equals
        /// </summary>
        Equals = 0,
        /// <summary>
        /// Value indicating it is not equals
        /// </summary>
        NotEquals,
        /// <summary>
        /// Value indicating it is greater than
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Value indicating it is greater than or equal
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// Value indicating it is less than
        /// </summary>
        LessThan,
        /// <summary>
        /// Value indicating it is less than or equal
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// Value indicating it is contains
        /// </summary>
        Contains,
        /// <summary>
        /// Value indicating it is bit or
        /// </summary>
        BitOr,
        /// <summary>
        /// Value indicating it is bit and
        /// </summary>
        BitAnd,
        /// <summary>
        /// Value indicating it is regex
        /// </summary>
        Regex,
        /// <summary>
        /// Value indicating it is search
        /// </summary>
        Search
    }
}
