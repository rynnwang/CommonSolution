using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Interface IRelationshipOperatorComputable
    /// </summary>
    public interface IRelationshipOperatorComputable : IExpression<IBooleanComputable, IBooleanComputable, RelationshipOperator>, IBooleanComputable
    {
    }
}
