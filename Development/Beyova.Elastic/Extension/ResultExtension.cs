using System;
using System.Collections.Generic;
using Beyova.ApiTracking;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ResultExtension.
    /// </summary>
    public static class ResultExtension
    {
        /// <summary>
        /// To the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryResult">The query result.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> ToEntityList<T>(this QueryResult<T> queryResult)
        {
            return ToEntityList(queryResult, x => x);
        }

        /// <summary>
        /// To the list.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <typeparam name="TResult">The type of the t result.</typeparam>
        /// <param name="queryResult">The query result.</param>
        /// <param name="converter">The converter.</param>
        /// <returns>List&lt;TResult&gt;.</returns>
        public static List<TResult> ToEntityList<TEntity, TResult>(this QueryResult<TEntity> queryResult, Func<TEntity, TResult> converter)
        {
            List<TResult> result = new List<TResult>();

            if (queryResult != null && converter != null)
            {
                try
                {
                    foreach (var item in queryResult.Hits)
                    {
                        result.AddIfNotNull(converter(item.Source));
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(queryResult);
                }
            }

            return result;
        }
    }
}
