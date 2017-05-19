using System;
using System.Text;
using System.Web;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityExtension.
    /// </summary>
    internal static class GravityExtension
    {
        #region Client Usages

        /// <summary>
        /// Converts to secured message package.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObject">The message object.</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns>SecuredMessagePackage.</returns>
        internal static SecuredMessagePackage ConvertToSecuredMessagePackage<T>(this SecuredMessageRequest<T> messageObject, string publicKey)
        {
            try
            {
                messageObject.CheckNullObject(nameof(messageObject));
                publicKey.CheckEmptyString(nameof(publicKey));

                messageObject.EncryptionKey.CheckNullOrEmptyCollection(nameof(messageObject.EncryptionKey));

                return new SecuredMessagePackage
                {
                    Security = EncodingOrSecurityExtension.RsaEncrypt(messageObject.EncryptionKey, publicKey),
                    Data = EncodingOrSecurityExtension.EncryptTripleDES(Encoding.UTF8.GetBytes(messageObject.Message.ToJson(false)), messageObject.EncryptionKey)
                };
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { messageObject, publicKey });
            }
        }

        /// <summary>
        /// Secures HTTP invoke.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="data">The data.</param>
        /// <param name="rsaPublicKey">The RSA public key.</param>
        /// <param name="token">The token.</param>
        /// <returns>System.String.</returns>
        internal static SecuredMessageObject<TOutput> SecureHttpInvoke<TInput, TOutput>(this Uri uri, TInput data, string rsaPublicKey, string token)
        {
            if (uri != null && data != null && !string.IsNullOrWhiteSpace(rsaPublicKey))
            {
                try
                {
                    var httpRequest = uri.CreateHttpWebRequest(HttpConstants.HttpMethod.Post);
                    httpRequest.SafeSetHttpHeader(HttpConstants.HttpHeader.TOKEN, token);

                    var tripleDesKey = EncodingOrSecurityExtension.GenerateTripleDESKey();

                    var package = new SecuredMessageRequest<TInput>
                    {
                        EncryptionKey = tripleDesKey,
                        Message = new SecuredMessageObject<TInput>
                        {
                            Data = data
                        }
                    };

                    httpRequest.FillData(HttpConstants.HttpMethod.Post, package.ConvertToSecuredMessagePackage(rsaPublicKey).ToBytes());

                    var responseBytes = httpRequest.ReadResponseAsBytes();
                    return responseBytes.ConvertToSecuredMessageObject<TOutput>(tripleDesKey);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { uri, rsaPublicKey });
                }
            }

            return null;
        }

        /// <summary>
        /// Converts to secured message object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes">The bytes.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        /// <returns>SecuredMessageObject&lt;T&gt;.</returns>
        internal static SecuredMessageObject<T> ConvertToSecuredMessageObject<T>(this byte[] bytes, byte[] encryptionKey)
        {
            SecuredMessageObject<T> result = null;

            if (bytes.HasItem() && encryptionKey.HasItem())
            {
                result = Encoding.UTF8.GetString(bytes.DecryptTripleDES(encryptionKey)).TryDeserializeAsObject<SecuredMessageObject<T>>();
            }

            return result;
        }

        #endregion

        #region Server Usages

        /// <summary>
        /// To bytes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageObject">The message object.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        /// <returns>System.Byte[].</returns>
        internal static byte[] ToBytes<T>(SecuredMessageObject<T> messageObject, byte[] encryptionKey)
        {
            if (messageObject != null && encryptionKey.HasItem())
            {
                return Encoding.UTF8.GetBytes(messageObject.ToJson(false)).EncryptTripleDES(encryptionKey);
            }

            return null;
        }

        /// <summary>
        /// Converts to secured message request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="package">The package.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>SecuredMessageRequest&lt;T&gt;.</returns>
        internal static SecuredMessageRequest<T> ConvertToSecuredMessageRequest<T>(this SecuredMessagePackage package, string privateKey)
        {
            try
            {
                package.CheckNullObject(nameof(package));
                privateKey.CheckNullObject(nameof(privateKey));

                package.Security.CheckNullOrEmptyCollection(nameof(package.Security));
                package.Data.CheckNullOrEmptyCollection(nameof(package.Data));

                var encryptionKey = package.Security.RsaDecrypt(privateKey);
                encryptionKey.CheckNullOrEmptyCollection(nameof(encryptionKey));

                return new SecuredMessageRequest<T>
                {
                    EncryptionKey = encryptionKey,
                    Message = Encoding.UTF8.GetString(package.Data.DecryptTripleDES(encryptionKey)).TryDeserializeAsObject<SecuredMessageObject<T>>()
                };
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { package, privateKey });
            }
        }

        /// <summary>
        /// Gets the secured message request.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>SecuredMessageRequest&lt;T&gt;.</returns>
        internal static SecuredMessageRequest<T> GetSecuredMessageRequest<T>(this HttpRequest httpRequest, string privateKey)
        {
            try
            {
                httpRequest.CheckNullObject(nameof(httpRequest));
                privateKey.CheckEmptyString(nameof(privateKey));

                var requestData = httpRequest.GetPostData();
                var package = SecuredMessagePackage.FromBytes(requestData);
                return ConvertToSecuredMessageRequest<T>(package, privateKey);
            }
            catch (Exception ex)
            {
                throw ex.Handle(data: new { privateKey });
            }
        }

        /// <summary>
        /// Responses the secure communication package.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response">The response.</param>
        /// <param name="responseObject">The response object.</param>
        /// <param name="encryptionKey">The encryption key.</param>
        internal static void ResponseSecureCommunicationPackage<T>(this HttpResponse response, T responseObject, byte[] encryptionKey)
        {
            try
            {
                response.CheckNullObject(nameof(response));
                responseObject.CheckNullObject(nameof(responseObject));
                encryptionKey.CheckNullOrEmptyCollection(nameof(encryptionKey));

                var responseBodyBytes = ToBytes(new SecuredMessageObject<T> { Data = responseObject }, encryptionKey);
                response.OutputStream.Write(responseBodyBytes, 0, responseBodyBytes.Length);
            }
            catch (Exception ex)
            {
                throw ex.Handle(data: new { responseObject, encryptionKey });
            }
        }

        /// <summary>
        /// Processes the secure HTTP invoke.
        /// </summary>
        /// <typeparam name="TInput">The type of the t input.</typeparam>
        /// <typeparam name="TOutput">The type of the t output.</typeparam>
        /// <typeparam name="TGravityClientObject">The type of the t gravity client object.</typeparam>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="getToken">The get token.</param>
        /// <param name="processFunc">The process function.</param>
        /// <param name="getClientObjectByToken">The get client object by token.</param>
        /// <param name="omitStampValidation">The omit stamp validation.</param>
        /// <returns>Beyova.ExceptionSystem.BaseException.</returns>
        internal static BaseException ProcessSecureHttpInvoke<TInput, TOutput, TGravityClientObject>(this HttpContext httpContext,
            Func<HttpRequest, string> getToken,
            Func<TInput, TOutput> processFunc,
            Func<string, TGravityClientObject> getClientObjectByToken,
            bool omitStampValidation = false)
            where TGravityClientObject : class, IRsaKeys
        {
            if (httpContext != null)
            {
                if (getToken == null)
                {
                    getToken = (r) => { return r?.TryGetHeader(HttpConstants.HttpHeader.TOKEN); };
                }

                string token = null;

                try
                {
                    token = getToken(httpContext.Request);

                    TGravityClientObject clientObject = null;
                    if (!string.IsNullOrWhiteSpace(token) && getClientObjectByToken != null)
                    {
                        clientObject = getClientObjectByToken(token);
                    }

                    clientObject.CheckNullObject(nameof(clientObject));
                    clientObject.PrivateKey.CheckNullObject(nameof(clientObject.PrivateKey));

                    var messageRequest = GetSecuredMessageRequest<TInput>(httpContext.Request, clientObject.PrivateKey);
                    var inputMessageObject = messageRequest.Message;

                    inputMessageObject.CheckNullObject(nameof(inputMessageObject));
                    inputMessageObject.Data.CheckNullObject("inputMessageObject.Data");

                    if (!omitStampValidation && !inputMessageObject.ValidateStamp())
                    {
                        throw ExceptionFactory.CreateInvalidObjectException(nameof(messageRequest), reason: "Stamp Invalid.");
                    }

                    var ouput = processFunc(inputMessageObject.Data);
                    httpContext.Response.ResponseSecureCommunicationPackage<TOutput>(ouput, messageRequest.EncryptionKey);
                }
                catch (Exception ex)
                {
                    return ex.Handle(new { token, url = httpContext.Request.RawUrl });
                }
            }

            return null;
        }

        #endregion
    }
}
