using System;
using System.Collections.Generic;
using Beyova.CommonService.DataAccessController;
using Beyova.CommonServiceInterface;

namespace Beyova.CommonService
{
    /// <summary>
    /// Class BaseProvisioningServiceCore.
    /// </summary>
    /// <typeparam name="TProvisioning">The type of the t provisioning.</typeparam>
    /// <typeparam name="TProvisioningCriteria">The type of the t provisioning criteria.</typeparam>
    /// <typeparam name="TApplication">The type of the t application.</typeparam>
    /// <typeparam name="TProvisioningAccessController">The type of the t provisioning access controller.</typeparam>
    public abstract class BaseProvisioningServiceCore<TProvisioning, TProvisioningCriteria, TApplication, TProvisioningAccessController> 
        : IProvisioningService<TProvisioning, TProvisioningCriteria, TApplication>
        where TProvisioning : IProvisioningObject<TApplication>, new()
        where TProvisioningCriteria : IProvisioningCriteria<TApplication>, new()
        where TApplication : struct, IConvertible
        where TProvisioningAccessController : ProvisioningAccessController<TProvisioning, TProvisioningCriteria, TApplication>, new()
    {
        /// <summary>
        /// Queries the global provisioning object.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TProvisioning&gt;.</returns>
        public List<TProvisioning> QueryGlobalProvisioningObject(TProvisioningCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");
                criteria.OwnerKey = null;

                using (var controller = new TProvisioningAccessController())
                {
                    return controller.QueryProvisioningObject(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryGlobalProvisioningItem", criteria);
            }
        }

        /// <summary>
        /// Queries the user provisioning object.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TProvisioning&gt;.</returns>
        public List<TProvisioning> QueryUserProvisioningObject(TProvisioningCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject("criteria");
                criteria.OwnerKey.CheckNullObject("criteria.OwnerKey");

                using (var controller = new TProvisioningAccessController())
                {
                    return controller.QueryProvisioningObject(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("QueryUserProvisioningObject", criteria);
            }
        }

        /// <summary>
        /// Saves the global provisioning object.
        /// </summary>
        /// <param name="provisioningObject">The provisioning object.</param>
        public void SaveGlobalProvisioningObject(TProvisioning provisioningObject)
        {
            try
            {
                provisioningObject.CheckNullObject("provisioningObject");
                provisioningObject.OwnerKey = null;

                using (var controller = new TProvisioningAccessController())
                {
                    controller.SaveProvisioningObject(provisioningObject, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("SaveGlobalProvisioningObject", provisioningObject);
            }
        }

        /// <summary>
        /// Saves the user provisioning object.
        /// </summary>
        /// <param name="provisioningObject">The provisioning item object.</param>
        public void SaveUserProvisioningObject(TProvisioning provisioningObject)
        {
            try
            {
                provisioningObject.CheckNullObject("provisioningObject");
                provisioningObject.OwnerKey.CheckNullObject("provisioningObject.OwnerKey");

                using (var controller = new TProvisioningAccessController())
                {
                    controller.SaveProvisioningObject(provisioningObject, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle("SaveUserProvisioningObject", provisioningObject);
            }
        }
    }
}
