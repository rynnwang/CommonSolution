using System;
using System.Collections.Generic;
using System.Text;
using Beyova.ExceptionSystem;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Class BooleanSearchExpressionReader.
    /// </summary>
    public class BooleanSearchExpressionReader
    {
        const char leftParentheses = '(';

        const char rightParentheses = ')';

        const char space = ' ';

        static char[] keyInterruptChars = new char[] { '!', '=', '<', '>', '$', '^', space };

        static char[] computeOperatorInterruptChars = new char[] { space, StringConstants.SingleQuoteChar, StringConstants.DoubleQuoteChar };

        static char[] valuerInterruptChars = new char[] { space, rightParentheses };

        static char[] relationshipOperatorInterruptChars = new char[] { space, leftParentheses, rightParentheses };

        static char[] computeOperatorSymbolChars = new char[] { '!', '=', '<', '>', '$', '^' };

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanSearchExpressionReader"/> class.
        /// </summary>
        public BooleanSearchExpressionReader()
        {
        }

        /// <summary>
        /// Reads as object.
        /// </summary>
        /// <param name="expressionString">The expression string.</param>
        /// <returns>IBooleanComputable.</returns>
        public IBooleanComputable ReadAsObject(string expressionString)
        {
            IBooleanComputable result = null;

            if (!string.IsNullOrWhiteSpace(expressionString))
            {
                int parenthesesDepth = 0;
                int position = 0;
                result = ExpectExpression(expressionString, ref parenthesesDepth, ref position);

                if (parenthesesDepth != 0)
                {
                    throw new InvalidExpressiontException(nameof(rightParentheses), data: expressionString, position: expressionString.Length - 1);
                }
            }

            return result;
        }


        /// <summary>
        /// Expects the expression.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="parenthesesDepth">The parentheses depth.</param>
        /// <param name="position">The position.</param>
        /// <returns>IBooleanComputable.</returns>
        /// <exception cref="Beyova.ExceptionSystem.InvalidExpressiontException">
        /// </exception>
        private IBooleanComputable ExpectExpression(string input, ref int parenthesesDepth, ref int position)
        {
            IBooleanComputable pendingExpression = null;
            RelationshipOperator? pendingRelationshipOperator = null;

            TrimStartSpaces(input, ref position);

            while (position < input.Length)
            {
                switch (input[position])
                {
                    case leftParentheses:
                        if (pendingExpression == null)
                        {
                            parenthesesDepth++;
                            position++;
                            pendingExpression = ExpectExpression(input, ref parenthesesDepth, ref position);
                        }
                        else
                        {
                            if (pendingRelationshipOperator.HasValue)
                            {
                                parenthesesDepth++;
                                position++;
                                pendingExpression = new RelationshipOperatorComputable
                                {
                                    Item1 = pendingExpression,
                                    Item2 = ExpectExpression(input, ref parenthesesDepth, ref position),
                                    Operator = pendingRelationshipOperator.Value
                                };

                                //Clean tmp status
                                pendingRelationshipOperator = null;
                            }
                            else
                            {
                                throw new InvalidExpressiontException(nameof(RelationshipOperator), data: input, position: position);
                            }
                        }
                        break;
                    case rightParentheses:
                        if (parenthesesDepth > 0)
                        {
                            if (pendingRelationshipOperator.HasValue)
                            {
                                throw new InvalidExpressiontException("Expression", data: input, position: position);
                            }

                            parenthesesDepth--;
                            position++;
                            return pendingExpression;
                        }
                        else
                        {
                            throw new InvalidExpressiontException(nameof(leftParentheses), data: input, position: position);
                        }
                        //break;
                    case space:
                        position++;
                        break;
                    default:
                        if (pendingExpression == null)
                        {
                            pendingExpression = ExpectPureKeyValueExpression(input, ref parenthesesDepth, ref position);
                        }
                        else
                        {
                            if (pendingRelationshipOperator.HasValue)
                            {
                                pendingExpression = new RelationshipOperatorComputable
                                {
                                    Item1 = pendingExpression,
                                    Item2 = ExpectExpression(input, ref parenthesesDepth, ref position),
                                    Operator = pendingRelationshipOperator.Value
                                };

                                //Clean tmp status
                                pendingRelationshipOperator = null;
                            }
                            else
                            {
                                var relationshipOperatorString = GetNextValidSection(input, ref position, false, relationshipOperatorInterruptChars);
                                RelationshipOperator relationshipOperator;
                                if (!IsRelationshipOperator(relationshipOperatorString, out relationshipOperator))
                                {
                                    throw new InvalidExpressiontException(nameof(RelationshipOperator), data: input, position: position);
                                }
                                else
                                {
                                    pendingRelationshipOperator = relationshipOperator;
                                }
                            }
                        }

                        break;
                }
            }

            return pendingExpression;
        }

        /// <summary>
        /// Expects the pure key value expression.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="parenthesesDepth">The parentheses depth.</param>
        /// <param name="position">The position.</param>
        /// <returns>CriteriaOperatorComputable.</returns>
        /// <exception cref="InvalidExpressiontException"></exception>
        private CriteriaOperatorComputable ExpectPureKeyValueExpression(string input, ref int parenthesesDepth, ref int position)
        {
            TrimStartSpaces(input, ref position);

            var key = GetNextValidSection(input, ref position, false, keyInterruptChars);

            var computeOperator = ExpectComputeOperator(input, ref position);
            if (!computeOperator.HasValue)
            {
                throw new InvalidExpressiontException(nameof(ComputeOperator), data: input, position: position);
            }

            var valueString = GetNextValidSection(input, ref position, true, valuerInterruptChars);
            return new CriteriaOperatorComputable
            {
                Item1 = key,
                Operator = computeOperator.Value,
                Item2 = valueString
            };
        }

        /// <summary>
        /// Expects the relationship operator.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="parenthesesDepth">The parentheses depth.</param>
        /// <param name="position">The position.</param>
        /// <returns>RelationshipOperator.</returns>
        private RelationshipOperator? ExpectRelationshipOperator(string input, ref int parenthesesDepth, ref int position)
        {
            string operatorString = null;

            TrimStartSpaces(input, ref position);

            int startPosition = position;

            while (position < input.Length)
            {
                if (!relationshipOperatorInterruptChars.Contains(input[position]))
                {
                    position++;
                }
                else
                {
                    break;
                }
            }

            operatorString = input.Substring(startPosition, position - startPosition);

            switch (operatorString.SafeToLower())
            {
                case "and":
                    return RelationshipOperator.And;
                case "or":
                    return RelationshipOperator.Or;
            }

            return null;
        }

        /// <summary>
        /// Expects the compute operator.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="position">The position.</param>
        /// <returns>System.Nullable&lt;ComputeOperator&gt;.</returns>
        /// <exception cref="Beyova.ExceptionSystem.InvalidExpressiontException"></exception>
        private ComputeOperator? ExpectComputeOperator(string input, ref int position)
        {
            string operatorString = null;

            TrimStartSpaces(input, ref position);
            var startPosition = position;
            bool isSymbol = position < input.Length && computeOperatorSymbolChars.Contains(input[position]);

            while (position < input.Length)
            {
                if (isSymbol)
                {
                    if (computeOperatorSymbolChars.Contains(input[position]))
                    {
                        position++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (!relationshipOperatorInterruptChars.Contains(input[position]))
                    {
                        position++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            operatorString = input.Substring(startPosition, position - startPosition);

            return BooleanSearchCore.ToComputeOperator(operatorString);
        }

        /// <summary>
        /// Determines whether [is compute operator] [the specified input].
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="computeOperator">The compute operator.</param>
        /// <returns><c>true</c> if [is compute operator] [the specified input]; otherwise, <c>false</c>.</returns>
        private bool IsComputeOperator(string input, out ComputeOperator computeOperator)
        {
            var result = BooleanSearchCore.ToComputeOperator(input);
            if (result.HasValue)
            {
                computeOperator = result.Value;
                return true;
            }

            computeOperator = default(ComputeOperator);
            return false;
        }

        /// <summary>
        /// Determines whether [is relationship operator] [the specified input].
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="relationshipOperator">The relationship operator.</param>
        /// <returns><c>true</c> if [is relationship operator] [the specified input]; otherwise, <c>false</c>.</returns>
        private bool IsRelationshipOperator(string input, out RelationshipOperator relationshipOperator)
        {
            switch (input.SafeToLower())
            {
                case "and":
                    relationshipOperator = RelationshipOperator.And;
                    return true;
                case "or":
                    relationshipOperator = RelationshipOperator.Or;
                    return true;
            }

            relationshipOperator = RelationshipOperator.And;
            return false;
        }

        /// <summary>
        /// Trims the start spaces.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="position">The position.</param>
        private void TrimStartSpaces(string input, ref int position)
        {
            while (position < input.Length)
            {
                if (input[position] == space)
                {
                    position++;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the next valid section.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="position">The position.</param>
        /// <param name="allowQuote">if set to <c>true</c> [allow quote].</param>
        /// <param name="interruptChars">The interrupt chars.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="InvalidExpressiontException">
        /// operator
        /// or
        /// </exception>
        private string GetNextValidSection(string input, ref int position, bool allowQuote, char[] interruptChars)
        {
            Char? quoteChar = null;
            string result = null;

            quoteChar = input[position];

            if (quoteChar.Value == StringConstants.SingleQuoteChar || quoteChar.Value == StringConstants.DoubleQuoteChar)
            {
                if (!allowQuote)
                {
                    throw new InvalidExpressiontException("Non-Quote", data: input, position: position);
                }

                position++;
            }
            else
            {
                quoteChar = null;
                TrimStartSpaces(input, ref position);
            }

            int startPosition = position;

            while (position < input.Length)
            {
                if (quoteChar.HasValue)
                {
                    if (input[position] == quoteChar.Value)
                    {
                        position++;
                        result = input.Substring(startPosition, position - startPosition);
                        break;
                    }
                    else
                    {
                        position++;
                    }
                }
                else
                {
                    if (interruptChars.Contains(input[position]))
                    {
                        result = input.Substring(startPosition, position - startPosition);
                        break;
                    }
                    else
                    {
                        position++;
                    }
                }
            }

            if (position >= input.Length)
            {
                if (quoteChar.HasValue)
                {
                    throw new InvalidExpressiontException(string.Format("Quote[{0}]", quoteChar.Value), data: input, position: position);
                }
                else
                {
                    result = input.Substring(startPosition);
                }
            }

            return result;
        }
    }
}
