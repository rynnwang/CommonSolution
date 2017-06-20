using System;
using System.Globalization;
using System.Net;

namespace Beyova.RestApi
{
    /// <summary>
    /// Class EntitySynchronizationModeAttribute. Limitation: Method output must is <see cref="ISimpleBaseObject"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EntitySynchronizationModeAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets if modified since key.
        /// </summary>
        /// <value>If modified since key.</value>
        public string IfModifiedSinceKey { get; set; }

        /// <summary>
        /// Gets or sets the last modified key.
        /// </summary>
        /// <value>The last modified key.</value>
        public string LastModifiedKey { get; set; }

        /// <summary>
        /// Gets or sets the stamp output function.
        /// </summary>
        /// <value>The stamp output function.</value>
        public Func<DateTime?, string> StampOutputFunction { get; protected set; }

        /// <summary>
        /// Gets or sets the stamp input function.
        /// </summary>
        /// <value>The stamp input function.</value>
        public Func<string, DateTime?> StampInputFunction { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySynchronizationModeAttribute" /> class.
        /// </summary>
        /// <param name="stampFormat">The stamp format.</param>
        /// <param name="ifModifiedSinceKey">If modified since key.</param>
        /// <param name="lastModifiedKey">The last modified key.</param>
        public EntitySynchronizationModeAttribute(string stampFormat, string ifModifiedSinceKey = null, string lastModifiedKey = null)
          : this(ifModifiedSinceKey, lastModifiedKey,
                string.IsNullOrWhiteSpace(stampFormat) ? DefaultStampOutputFunc : new Func<DateTime?, string>((dt) => { return dt.HasValue ? dt.Value.ToString(stampFormat) : string.Empty; }),
               string.IsNullOrWhiteSpace(stampFormat) ? DefaultStampInputFunc : new Func<string, DateTime?>(s => { return string.IsNullOrWhiteSpace(s) ? default(DateTime?) : DateTime.ParseExact(s, stampFormat, CultureInfo.InvariantCulture); })
               )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntitySynchronizationModeAttribute" /> class.
        /// </summary>
        /// <param name="ifModifiedSinceKey">If modified since key.</param>
        /// <param name="lastModifiedKey">The last modified key.</param>
        /// <param name="stampOutputFunc">The stamp output function.</param>
        /// <param name="stampInputFunc">The stamp input function.</param>
        public EntitySynchronizationModeAttribute(string ifModifiedSinceKey = null, string lastModifiedKey = null, Func<DateTime?, string> stampOutputFunc = null, Func<string, DateTime?> stampInputFunc = null)
            : base()
        {
            this.IfModifiedSinceKey = ifModifiedSinceKey.SafeToString(HttpConstants.HttpHeader.XIfModifiedSince);
            this.LastModifiedKey = lastModifiedKey.SafeToString(HttpConstants.HttpHeader.XLastModified);
            this.StampInputFunction = stampInputFunc;
            this.StampOutputFunction = stampOutputFunc;
        }

        #region RebuildOutputObject

        /// <summary>
        /// Rebuilds the output object.
        /// </summary>
        /// <param name="lastModifiedSinceStamp">The last modified since stamp.</param>
        /// <param name="data">The data.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="noBody">if set to <c>true</c> [no body].</param>
        /// <param name="lastModifiedStamp">The last modified stamp.</param>
        /// <returns>System.Object.</returns>
        public object RebuildOutputObject(DateTime? lastModifiedSinceStamp, object data, ref int httpStatusCode, ref bool noBody, out DateTime? lastModifiedStamp)
        {
            ISimpleBaseObject simpleObject = data as ISimpleBaseObject;
            if (simpleObject != null)
            {
                return InternalRebuildOutputObject(lastModifiedSinceStamp, simpleObject, ref httpStatusCode, ref noBody, out lastModifiedStamp);
            }

            lastModifiedStamp = null;
            return data;
        }

        /// <summary>
        /// Rebuilds the output object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lastModifiedSinceStamp">The last modified since stamp.</param>
        /// <param name="data">The data.</param>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="noBody">if set to <c>true</c> [no body].</param>
        /// <param name="lastModifiedStamp">The last modified stamp.</param>
        /// <returns>System.Object.</returns>
        internal object InternalRebuildOutputObject<T>(DateTime? lastModifiedSinceStamp, T data, ref int httpStatusCode, ref bool noBody, out DateTime? lastModifiedStamp) where T : ISimpleBaseObject
        {
            lastModifiedStamp = null;

            if (lastModifiedSinceStamp.HasValue && data != null && data.LastUpdatedStamp.HasValue && data.LastUpdatedStamp.Value < lastModifiedSinceStamp.Value)
            {
                noBody = true;
                httpStatusCode = (int)HttpStatusCode.NotModified;
                return null;
            }

            return data;
        }

        #endregion RebuildOutputObject

        /// <summary>
        /// Determines whether [is return type matched] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Boolean.</returns>
        public static bool IsReturnTypeMatched(Type type)
        {
            return type != null && type.IsAssignableFrom(typeof(ISimpleBaseObject));
        }

        #region Default Func

        /// <summary>
        /// Defaults the stamp input function.
        /// </summary>
        /// <param name="stamp">The stamp.</param>
        /// <returns>System.Nullable&lt;System.DateTime&gt;.</returns>
        protected static DateTime? DefaultStampInputFunc(string stamp)
        {
            return string.IsNullOrWhiteSpace(stamp) ? default(DateTime?) : stamp.ObjectToDateTime();
        }

        /// <summary>
        /// Defaults the stamp output function.
        /// </summary>
        /// <param name="stamp">The stamp.</param>
        /// <returns>System.String.</returns>
        protected static string DefaultStampOutputFunc(DateTime? stamp)
        {
            return stamp.HasValue ? stamp.ToFullDateTimeTzString() : null;
        }

        #endregion Default Func
    }
}