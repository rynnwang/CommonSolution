using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Beyova;
using Newtonsoft.Json.Linq;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Class BooleanSearchCore.
    /// </summary>
    public static class BooleanSearchCore
    {
        /// <summary>
        /// The criteria key regex
        /// </summary>
        static readonly Regex criteriaKeyRegex = new Regex(@"^[a-zA-Z0-9\/_-]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The compute operator strings.
        /// </summary>
        static readonly string[] computeOperatorStrings = new string[] { "=", "<>", ">=", "<=", ">", "<", "^", "$", "Contains", "NotContains", "Exists" };

        /// <summary>
        /// The expression format
        /// </summary>
        internal const string expressionFormat = "({0} {1} {2})";

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
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        public static bool BooleanCompute(ICriteriaOperatorComputable criteriaOperatorComputable, JObject json)
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
                            result = ComputeAsArray((JArray)jToken, criteriaOperatorComputable.Item2, criteriaOperatorComputable.Operator);
                            break;
                        case JTokenType.Date:
                            result = Compute(jToken.ToObject<DateTime>(), Convert.ToDateTime(criteriaOperatorComputable.Item2), criteriaOperatorComputable.Operator);
                            break;
                        case JTokenType.Uri:
                        case JTokenType.String:
                            result = ComputeAsString(jToken.ToObject<string>(), criteriaOperatorComputable.Item2, criteriaOperatorComputable.Operator);
                            break;
                        case JTokenType.Integer:
                            result = Compute(jToken.ToObject<long>(), criteriaOperatorComputable.Item2.ToInt64(), criteriaOperatorComputable.Operator);
                            break;
                        case JTokenType.Guid:
                            result = Compute(jToken.ToObject<Guid>(), criteriaOperatorComputable.Item2.ToGuid().Value, criteriaOperatorComputable.Operator);
                            break;
                        case JTokenType.Float:
                            result = Compute(jToken.ToObject<double>(), criteriaOperatorComputable.Item2.ToDouble(), criteriaOperatorComputable.Operator);
                            break;
                        case JTokenType.Boolean:
                            result = ComputeAsBoolean(jToken.ToObject<bool>(), criteriaOperatorComputable.Item2.ToBoolean(), criteriaOperatorComputable.Operator);
                            break;
                        case JTokenType.Object:
                            result = ComputeAsObject((JObject)jToken, criteriaOperatorComputable.Item2, criteriaOperatorComputable.Operator);
                            break;
                        default:
                            break;
                    }
                }
            }

            return result;
        }

        #region Process compute

        /// <summary>
        /// Computes as string.
        /// </summary>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="computeOperator">The compute operator.</param>
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        private static bool ComputeAsString(string item1, string item2, ComputeOperator computeOperator)
        {
            switch (computeOperator)
            {
                case ComputeOperator.EndWith:
                    return !string.IsNullOrEmpty(item1) && !string.IsNullOrEmpty(item2) && item1.EndsWith(item2);
                case ComputeOperator.StartWith:
                    return !string.IsNullOrEmpty(item1) && !string.IsNullOrEmpty(item2) && item1.StartsWith(item2);
                case ComputeOperator.Equals:
                    return !string.IsNullOrEmpty(item1) && !string.IsNullOrEmpty(item2) && item1.Equals(item2);
                case ComputeOperator.NotEquals:
                    return !string.IsNullOrEmpty(item1) && !string.IsNullOrEmpty(item2) && !item1.Equals(item2);
                case ComputeOperator.Contains:
                    return !string.IsNullOrEmpty(item1) && !string.IsNullOrEmpty(item2) && item1.Contains(item2);
                case ComputeOperator.NotContains:
                    return !string.IsNullOrEmpty(item1) && !string.IsNullOrEmpty(item2) && !item1.Contains(item2);
                default:
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(computeOperator), new
                    {
                        ComputeOperator = computeOperator.EnumToString()
                    });
            }
        }

        /// <summary>
        /// Computes as boolean.
        /// </summary>
        /// <param name="item1">if set to <c>true</c> [item1].</param>
        /// <param name="item2">if set to <c>true</c> [item2].</param>
        /// <param name="computeOperator">The compute operator.</param>
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        private static bool ComputeAsBoolean(bool item1, bool item2, ComputeOperator computeOperator)
        {
            switch (computeOperator)
            {
                case ComputeOperator.Equals:
                    return item1 == item2;
                case ComputeOperator.NotEquals:
                    return item1 != item2;
                default:
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(computeOperator), new
                    {
                        ComputeOperator = computeOperator.EnumToString()
                    });
            }
        }

        /// <summary>
        /// Computes as array.
        /// </summary>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="computeOperator">The compute operator.</param>
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        private static bool ComputeAsArray(JArray item1, string item2, ComputeOperator computeOperator)
        {
            switch (computeOperator)
            {
                case ComputeOperator.Contains:
                    return item1.Contains<JToken, string>(item2, x => x.ToObject<string>(), (x, y) => x.Equals(y, StringComparison.OrdinalIgnoreCase));
                case ComputeOperator.NotContains:
                    return !item1.Contains<JToken, string>(item2, x => x.ToObject<string>(), (x, y) => x.Equals(y, StringComparison.OrdinalIgnoreCase));
                default:
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(computeOperator), new
                    {
                        ComputeOperator = computeOperator.EnumToString()
                    });
            }
        }

        /// <summary>
        /// Computes as object.
        /// </summary>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="computeOperator">The compute operator.</param>
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        private static bool ComputeAsObject(JObject item1, string item2, ComputeOperator computeOperator)
        {
            switch (computeOperator)
            {
                case ComputeOperator.Exists:
                    return item1 != null && !string.IsNullOrWhiteSpace(item2) && item1.GetProperty(item2) != null;
                default:
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(computeOperator), new
                    {
                        ComputeOperator = computeOperator.EnumToString()
                    });
            }
        }

        /// <summary>
        /// Computes the specified item1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="computeOperator">The compute operator.</param>
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        private static bool Compute<T>(T item1, T item2, ComputeOperator computeOperator) where T : struct, IComparable
        {
            switch (computeOperator)
            {
                case ComputeOperator.GreatThan:
                    return item1.CompareTo(item2) > 0;
                case ComputeOperator.GreatThanOrEquals:
                    return item1.CompareTo(item2) >= 0;
                case ComputeOperator.LessThan:
                    return item1.CompareTo(item2) < 0;
                case ComputeOperator.LessThanOrEquals:
                    return item1.CompareTo(item2) <= 0;
                case ComputeOperator.NotEquals:
                    return !item1.Equals(item2);
                case ComputeOperator.Equals:
                    return item1.Equals(item2);
                default:
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(computeOperator), new
                    {
                        ComputeOperator = computeOperator.EnumToString(),
                        Type = typeof(T).FullName
                    });
            }
        }

        #endregion

        /// <summary>
        /// Booleans the compute.
        /// </summary>
        /// <param name="operatorComputable">The operator computable.</param>
        /// <param name="json">The json.</param>
        /// <returns><c>true</c> if compute as true, <c>false</c> otherwise.</returns>
        public static bool BooleanCompute(IRelationshipOperatorComputable operatorComputable, JObject json)
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

        /// <summary>
        /// To the expression string.
        /// </summary>
        /// <param name="computeOperator">The compute operator.</param>
        /// <returns>System.String.</returns>
        internal static string ToExpressionString(ComputeOperator computeOperator)
        {
            return computeOperatorStrings[(int)computeOperator];
        }

        /// <summary>
        /// To the compute operator.
        /// </summary>
        /// <param name="computeOperatorString">The compute operator string.</param>
        /// <returns>ComputeOperator.</returns>
        internal static ComputeOperator? ToComputeOperator(string computeOperatorString)
        {
            int index = -1;
            if (!string.IsNullOrWhiteSpace(computeOperatorString))
            {
                computeOperatorString = computeOperatorString.Trim();
                for (int i = 0; i < computeOperatorStrings.Length; i++)
                {
                    if (computeOperatorStrings[i].Equals(computeOperatorString, StringComparison.OrdinalIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }
            }

            return index > -1 ? (ComputeOperator?)index : null;
        }

        /// <summary>
        /// Strings the field to expression string.
        /// </summary>
        /// <param name="stringField">The string field.</param>
        /// <returns>System.String.</returns>
        internal static string StringFieldToExpressionString(string stringField)
        {
            if (string.IsNullOrWhiteSpace(stringField))
            {
                return "\"\"";
            }
            else if (stringField.Contains(StringConstants.WhiteSpace))
            {
                return string.Format(stringField.Contains(StringConstants.SingleQuoteChar) ? "\"{0}\"" : "'{0}'", stringField);
            }
            else
            {
                return stringField;
            }
        }


    }
}
