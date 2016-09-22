using System;
using System.Reflection;
using System.Web;
using Beyova.Gravity;
using Newtonsoft.Json.Linq;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class SecuredApiRouter.
    /// </summary>
    public abstract class SecuredApiRouter : RestApiRouter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecuredApiRouter"/> class.
        /// </summary>
        /// <param name="defaultApiSettings">The default API settings.</param>
        public SecuredApiRouter(RestApiSettings defaultApiSettings) : base(defaultApiSettings, false)
        {
        }

        /// <summary>
        /// Prepares the specified request.
        /// <remarks>
        /// This method would be called before <c>ProcessRoute</c>. It can be used to help you to do some preparation, such as get something from headers or cookie for later actions.
        /// ou can save them in Thread data so that you can get them later in <c>ProcessRoute</c>, <c>Invoke</c>, <c>PackageOutput</c> ,etc.
        /// If any exception is throw from this method, the process flow would be interrupted.
        /// </remarks>
        /// </summary>
        /// <param name="request">The request.</param>
        protected override void Prepare(HttpRequest request)
        {
            base.Prepare(request);

            var rsaPublicKey = request.TryGetHeader(HttpConstants.HttpHeader.SECUREKEY);
            if (string.IsNullOrWhiteSpace(rsaPublicKey))
            {
                throw ExceptionFactory.CreateUnauthorizedTokenException(rsaPublicKey);
            }

            var privateKey = GetPrivateKeyByPublicKey(rsaPublicKey);
            if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw ExceptionFactory.CreateUnauthorizedTokenException(rsaPublicKey);
            }

            var currentGravityContext = GravityContext.Current;
            currentGravityContext.PublicKey = rsaPublicKey;
            currentGravityContext.PrivateKey = privateKey;
        }

        /// <summary>
        /// Invokes the specified method information.
        /// <remarks>
        /// Invoke action would regard to method parameter to use difference logic. Following steps show the IF-ELSE case. When it is hit, other would not go through.
        /// <list type="number"><item>
        /// If input parameter count is 0, invoke without parameter object.
        /// </item><item>
        /// If input parameter count is 1 and key is not empty or null, invoke using key.
        /// </item><item>
        /// If input parameter count is 1 and key is empty or null, invoke using key, try to get JSON object from request body and convert to object for invoke.
        /// </item><item>
        /// If input parameter count more than 1, try read JSON data to match parameters by name (ignore case) in root level, then invoke.
        /// </item></list></remarks>
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="key">The key.</param>
        /// <param name="jsonBody">The json body.</param>
        /// <returns>
        /// System.Object.
        /// </returns>
        protected override object Invoke(object instance, MethodInfo methodInfo, HttpRequest httpRequest, string key, out string jsonBody)
        {
            try
            {
                var rsaPrivateKey = GravityContext.Current.PrivateKey;
                var securePackage = httpRequest.GetSecureCommunicationPackage<JToken>(rsaPrivateKey);
                securePackage.CheckNullObject(nameof(securePackage));

                var inputParameters = methodInfo.GetParameters();
                jsonBody = securePackage.ToString();

                if (inputParameters.Length == 0)
                {
                    return methodInfo.Invoke(instance, null);
                }
                else if (inputParameters.Length == 1)
                {
                    return methodInfo.Invoke(instance, new object[] { securePackage.Data.ToObject(inputParameters[0].ParameterType) });
                }
                else
                {
                    object[] parameters = new object[inputParameters.Length];

                    var jsonObject = securePackage.Data as JObject;

                    if (jsonObject != null)
                    {
                        for (int i = 0; i < inputParameters.Length; i++)
                        {
                            var jTokenObject = jsonObject.GetProperty(inputParameters[i].Name);
                            parameters[i] = jTokenObject == null ? null : jTokenObject.ToObject(inputParameters[i].ParameterType);
                        }
                    }

                    return methodInfo.Invoke(instance, parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { methodInfo = methodInfo?.GetFullName(), httpRequest = httpRequest?.RawUrl });
            }
        }

        /// <summary>
        /// Gets the private key by public key.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <returns></returns>
        protected abstract string GetPrivateKeyByPublicKey(string publicKey);
    }
}
