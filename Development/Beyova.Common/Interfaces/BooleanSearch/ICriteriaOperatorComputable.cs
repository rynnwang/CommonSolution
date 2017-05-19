using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Interface ICriteriaOperatorComputable
    /// </summary>
    public interface ICriteriaOperatorComputable : IExpression<string, string, ComputeOperator>, IBooleanComputable
    {
        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns><c>true</c> if validation passed, <c>false</c> otherwise.</returns>
        bool Validate();
    }
}
