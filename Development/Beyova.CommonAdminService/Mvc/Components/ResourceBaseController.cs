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
    public abstract class ResourceBaseController<TResource, TResourceCriteria, TResourceObject> : EnvironmentBaseController
        where TResource : BinaryStorageMetaBase
       where TResourceCriteria : BinaryStorageMetaDataCriteria
        where TResourceObject : BinaryStorageObject
    {
        /// <summary>
        /// The _view prefix name
        /// </summary>
        protected string _viewPrefixName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceBaseController" /> class.
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        /// <param name="viewPrefixName">Name of the view prefix.</param>
        public ResourceBaseController(string moduleCode, string viewPrefixName) : base(moduleCode)
        {
            _viewPrefixName = viewPrefixName.SafeToString();
        }

        // GET: Resource
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MetaData()
        {
            return View("StoragePanel");
        }
    }
}