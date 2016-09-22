using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Beyova;
using Newtonsoft.Json.Linq;

namespace Beyova.BooleanSearch
{
    public static class BooleanSearchCore
    {
        static Regex criteriaKeyRegex = new Regex(@"^[a-zA-Z0-9\/_-]$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Determines whether [is key valid].
        /// </summary>
        /// <param name="operatorComputable">The operator computable.</param>
        /// <returns>
        ///   <c>true</c> if [is key valid] [the specified operator computable]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyValid(this ICriteriaOperatorComputable operatorComputable)
        {
            return operatorComputable != null && criteriaKeyRegex.IsMatch(operatorComputable.Item1);
        }

        /// <summary>
        /// Booleans the compute.
        /// </summary>
        /// <param name="criteriaOperatorComputable">The criteria operator computable.</param>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static bool BooleanCompute(this ICriteriaOperatorComputable criteriaOperatorComputable, JObject json)
        {
            bool result = false;

            if (criteriaOperatorComputable != null && criteriaOperatorComputable.IsKeyValid() && json != null)
            {
                var jToken = json.XPath(criteriaOperatorComputable.Item1);
                if (jToken != null)
                {
                    switch (jToken.Type)
                    {
                        case JTokenType.Array:
                            break;
                        case JTokenType.Date:
                            break;
                        default:
                            break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Booleans the compute.
        /// </summary>
        /// <param name="operatorComputable">The operator computable.</param>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static bool BooleanCompute(this IRelationshipOperatorComputable operatorComputable, JObject json)
        {
            bool result = false;

            if (operatorComputable != null && operatorComputable.Item1 != null && operatorComputable.Item2 != null)
            {
                switch (operatorComputable.Operator)
                {
                    case RelationshipOperator.And:
                        result = operatorComputable.Item1.Compute(json) && operatorComputable.Item2.Compute(json);
                        break;
                    case RelationshipOperator.Or:
                        result = operatorComputable.Item1.Compute(json) || operatorComputable.Item2.Compute(json);
                        break;
                    default:
                        result = false;
                        break;
                }
            }

            return result;
        }
    }
}
