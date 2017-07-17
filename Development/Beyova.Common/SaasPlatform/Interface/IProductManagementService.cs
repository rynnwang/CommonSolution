using System;
using System.Collections.Generic;
using Beyova.Api;
using Beyova.RestApi;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// Interface IProductManagementService
    /// </summary>
    /// <typeparam name="TProductInfo">The type of the product information.</typeparam>
    /// <typeparam name="TProductCriteria">The type of the product criteria.</typeparam>
    [TokenRequired]
    public interface IProductManagementService<TProductInfo, TProductCriteria>
        where TProductInfo : SaasPlatform.ProductBase
        where TProductCriteria : SaasPlatform.ProductCriteria
    {
        #region Product

        /// <summary>
        /// Creates the or update product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.Product, HttpConstants.HttpMethod.Put)]
        [ApiPermission(CommonServiceConstants.Permission.Administrator, ApiPermission.Required)]
        Guid? CreateOrUpdateProduct(TProductInfo product);

        /// <summary>
        /// Queries the product information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TProductInfo&gt;.</returns>
        [ApiOperation(CommonServiceConstants.ResourceName.Product, HttpConstants.HttpMethod.Post)]
        [ApiPermission(CommonServiceConstants.Permission.Administrator, ApiPermission.Required)]
        List<TProductInfo> QueryProductInfo(TProductCriteria criteria);

        #endregion Product
    }
}