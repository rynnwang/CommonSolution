using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityBaseClient.
    /// </summary>
    internal abstract class GravityBaseClient
    {
        /// <summary>
        /// Gets or sets the entry.
        /// </summary>
        /// <value>The entry.</value>
        public GravityEntryObject Entry { get; protected set; }

        /// <summary>
        /// Gets or sets the event hook.
        /// </summary>
        /// <value>The event hook.</value>
        public GravityEventHook EventHook { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityBaseClient" /> class.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="hook">The hook.</param>
        internal GravityBaseClient(GravityEntryObject entry, GravityEventHook hook = null)
        {
            this.Entry = entry;
            this.EventHook = hook;
        }

        /// <summary>
        /// Invokes the specified feature.
        /// </summary>
        /// <typeparam name="TIn">The type of the t in.</typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="module">The module.</param>
        /// <param name="feature">The feature.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>TOut.</returns>
        protected TOut Invoke<TIn, TOut>(string module, string feature, TIn parameter)
        {
            BaseException exception = null;

            try
            {
                var responseMessage = GravityExtension.SecureHttpInvoke<TIn, TOut>(GetInvokeUri(module, feature), parameter, this.Entry.PublicKey, this.Entry.MemberIdentifiableKey);
                if (responseMessage == null || !responseMessage.ValidateStamp())
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(responseMessage), reason: "Stamp invalid.");
                }
                return responseMessage.Data;
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { feature, parameter });
                throw exception;
            }
            finally
            {
                if (EventHook != null)
                {
                    EventHook.OnSecuredMessageProcessedCompleted(feature, parameter, exception);
                }
            }
        }

        /// <summary>
        /// Gets the invoke URI.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="feature">The feature.</param>
        /// <returns>Uri.</returns>
        protected Uri GetInvokeUri(string module, string feature)
        {
            return new Uri(string.Format("{0}/?action={1}&module={2}", this.Entry.GravityServiceUri.ToString().TrimEnd('/'), feature, module));
        }
    }
}
