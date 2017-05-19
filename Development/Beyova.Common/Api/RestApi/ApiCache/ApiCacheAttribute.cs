using Beyova.Cache;
using Beyova.RestApi;
using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using Beyova;
using System.Collections.Generic;
using System.Text;

namespace Beyova.Cache
{
    /// <summary>
    /// Class ApiCacheAttribute. It would take effects when <see cref="ApiOperationAttribute"/> take effects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class ApiCacheAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the cache parameter.
        /// </summary>
        /// <value>The cache parameter.</value>
        public ApiCacheParameter CacheParameter { get; protected set; }

        /// <summary>
        /// The parameter names
        /// </summary>
        protected string[] _parameterNames;

        /// <summary>
        /// The default builder capacity
        /// </summary>
        protected int _defaultBuilderCapacity = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiCacheAttribute" /> class.
        /// </summary>
        /// <param name="cacheParameter">The cache parameter.</param>
        public ApiCacheAttribute(ApiCacheParameter cacheParameter)
        {
            this.CacheParameter = cacheParameter;
        }

        /// <summary>
        /// Generates the parameterized identity.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns>System.String.</returns>
        internal string GenerateParameterizedIdentity(NameValueCollection queryString)
        {
            if (_defaultBuilderCapacity > 0 && queryString != null)
            {
                if (_parameterNames.Length == 1)
                {
                    return queryString.Get(_parameterNames[0]);
                }
                else if (_parameterNames.Length == 2)
                {
                    return queryString.Get(_parameterNames[0]) + queryString.Get(_parameterNames[1]);
                }
                else
                {
                    StringBuilder builder = new StringBuilder(_defaultBuilderCapacity);

                    foreach (var name in _parameterNames)
                    {
                        builder.Append(queryString.Get(name));
                    }

                    return builder.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Initializes the parameter names.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <returns><c>true</c> if succeed to initialize and OK for use later, <c>false</c> otherwise.</returns>
        internal bool InitializeParameterNames(MethodInfo methodInfo)
        {
            if (methodInfo != null)
            {
                var parameterNames = new List<string>();
                foreach (var one in methodInfo.GetParameters())
                {
                    if (one.ParameterType.IsSimpleType())
                    {
                        parameterNames.Add(one.Name);
                    }
                    else
                    {
                        return false;
                    }
                }

                _parameterNames = parameterNames.ToArray();
                _defaultBuilderCapacity = _parameterNames.Length * 10;
                return true;
            }

            return false;
        }
    }
}
