using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova.BinaryStorage;

namespace Beyova.CommonAdminService
{
    /// <summary>
    /// Class ResourceBaseController.
    /// </summary>
    /// <typeparam name="TResource">The type of the t resource.</typeparam>
    /// <typeparam name="TResourceCriteria">The type of the t resource criteria.</typeparam>
    /// <typeparam name="TResourceObject">The type of the t resource object.</typeparam>
    public class BinaryStorageController : ResourceBaseController<BinaryStorageMetaData, BinaryStorageMetaDataCriteria, BinaryStorageObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceBaseController" /> class.
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        public BinaryStorageController() : base("BinaryStorage")
        {
        }

        protected override EnvironmentEndpoint GetEnvironmentEndpoint(string environment)
        {
            throw new NotImplementedException();
        }
    }
}