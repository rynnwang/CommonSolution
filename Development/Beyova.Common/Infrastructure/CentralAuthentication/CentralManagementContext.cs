using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class CentralManagementContext.
    /// </summary>
    public static class CentralManagementContext
    {
        /// <summary>
        /// </summary>
        [ThreadStatic]
        private static Guid? _parameterProductKey;

        /// <summary>
        /// Gets the parameter product key.
        /// </summary>
        /// <value>The parameter product key.</value>
        public static Guid? ParameterProductKey
        {
            get { return _parameterProductKey; }
            internal set { _parameterProductKey = value; }
        }

        /// <summary>
        /// Gets the product key.
        /// </summary>
        /// <value>The product key.</value>
        public static Guid? ProductKey
        {
            get
            {
                var adminUser = ContextHelper.CurrentUserInfo as AdminUserInfo;
                adminUser.CheckNullObject(nameof(adminUser));

                return adminUser.ProductKey ?? _parameterProductKey;
            }
        }

        /// <summary>
        /// Ensures the product scope.
        /// </summary>
        /// <param name="productBasedObject">The product based object.</param>
        /// <exception cref="UnauthorizedOperationException">ProductOwnership</exception>
        public static void EnsureProductScope(this IProductIdentifier productBasedObject)
        {
            if (productBasedObject != null)
            {
                var adminUser = ContextHelper.CurrentUserInfo as AdminUserInfo;
                adminUser.CheckNullObject(nameof(adminUser));

                if (_parameterProductKey.HasValue)
                {
                    if (!adminUser.ProductKey.HasValue || adminUser.ProductKey.Value == _parameterProductKey.Value)
                    {
                        productBasedObject.ProductKey = _parameterProductKey;
                    }
                    else
                    {
                        throw new UnauthorizedOperationException("ProductOwnership", new { ProductKey = productBasedObject?.ProductKey });
                    }
                }
                else
                {
                    productBasedObject.ProductKey = adminUser.ProductKey ?? productBasedObject.ProductKey;
                }
            }
        }

        /// <summary>
        /// Ensures the product scope.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="productBasedObjects">The product based objects.</param>
        /// <param name="forceApplyUrlParameterProductKey">if set to <c>true</c> [force apply URL parameter product key].</param>
        public static void EnsureProductScope<T>(this IEnumerable<T> productBasedObjects, bool forceApplyUrlParameterProductKey = false)
            where T : IProductIdentifier
        {
            if (productBasedObjects.HasItem())
            {
                var adminUser = ContextHelper.CurrentUserInfo as AdminUserInfo;
                adminUser.CheckNullObject(nameof(adminUser));

                foreach (var productBasedObject in productBasedObjects)
                {
                    if (forceApplyUrlParameterProductKey && _parameterProductKey.HasValue)
                    {
                        productBasedObject.ProductKey = _parameterProductKey;
                    }

                    productBasedObject.ProductKey = adminUser.ProductKey ?? productBasedObject.ProductKey;
                }
            }
        }

        /// <summary>
        /// Validates the product scope.
        /// </summary>
        /// <param name="productBasedObject">The product based object.</param>
        /// <returns>Return true if validation passed.</returns>
        public static bool ValidateProductScope(this IProductIdentifier productBasedObject)
        {
            if (productBasedObject != null)
            {
                var adminUser = ContextHelper.CurrentUserInfo as AdminUserInfo;
                adminUser.CheckNullObject(nameof(adminUser));

                if (!adminUser.ProductKey.HasValue || adminUser.ProductKey.Value == productBasedObject.ProductKey)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        internal static void Dispose()
        {
            _parameterProductKey = null;
        }
    }
}