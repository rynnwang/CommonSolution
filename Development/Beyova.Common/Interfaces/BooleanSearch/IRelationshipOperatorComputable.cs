namespace Beyova.BooleanSearch
{
    /// <summary>
    /// Interface IRelationshipOperatorComputable
    /// </summary>
    public interface IRelationshipOperatorComputable : IExpression<IBooleanComputable, IBooleanComputable, RelationshipOperator>, IBooleanComputable
    {
    }
}