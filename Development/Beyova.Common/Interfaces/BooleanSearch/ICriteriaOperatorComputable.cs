using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Interface ICriteriaOperatorComputable
    /// </summary>
    /// <seealso cref="Beyova.BooleanSearch.IExpression{System.String, System.String, Beyova.BooleanSearch.ComputeOperator}" />
    public interface ICriteriaOperatorComputable : IExpression<string, string, ComputeOperator>, IBooleanComputable
    {
        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns></returns>
        bool Validate();
    }
}
