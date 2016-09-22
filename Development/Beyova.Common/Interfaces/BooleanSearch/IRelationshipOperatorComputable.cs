using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Interface IRelationshipOperatorComputable
    /// </summary>
    /// <seealso cref="Beyova.BooleanSearch.IExpression{Beyova.BooleanSearch.IBooleanComputable, Beyova.BooleanSearch.IBooleanComputable, Beyova.BooleanSearch.RelationshipOperator}" />
    public interface IRelationshipOperatorComputable : IExpression<IBooleanComputable, IBooleanComputable, RelationshipOperator>, IBooleanComputable
    {
    }
}
