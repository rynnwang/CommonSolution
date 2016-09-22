using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class BaseException.
    /// </summary>
    public abstract class BaseException : System.Exception
    {
        #region Properties

        /// <summary>
        /// Gets or sets the hint.
        /// </summary>
        /// <value>The hint.</value>
        public FriendlyHint Hint { get; protected set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public ExceptionCode Code { get; protected set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid Key { get; internal set; }

        /// <summary>
        /// Gets or sets the reference data.
        /// </summary>
        /// <value>The reference data.</value>
        public JToken ReferenceData { get; protected set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime CreatedStamp { get; protected set; }

        /// <summary>
        /// Gets or sets the operator credential.
        /// </summary>
        /// <value>The operator credential.</value>
        public BaseCredential OperatorCredential { get; protected set; }

        /// <summary>
        /// Gets or sets the scene.
        /// </summary>
        /// <value>The scene.</value>
        public ExceptionScene Scene { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException" /> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="code">The exception code.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        protected BaseException(string message, ExceptionCode code, Exception innerException = null, object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(message, innerException)
        {
            this.Code = code;
            this.OperatorCredential = ContextHelper.CurrentCredential;
            this.ReferenceData = data == null ? null : JToken.FromObject(data);
            this.Scene = scene;
            this.CreatedStamp = DateTime.UtcNow;

            var inner = innerException as BaseException;
            this.Key = inner == null ? Guid.NewGuid() : inner.Key;
            this.Hint = hint ?? inner?.Hint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException" /> class. This is used internally for restoring exception instance from ExceptionInfo.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createdStamp">The created stamp.</param>
        /// <param name="message">The message.</param>
        /// <param name="scene">The scene.</param>
        /// <param name="code">The code.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="operatorCredential">The operator credential.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        protected BaseException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
                     : base(message, innerException)
        {
            this.Key = key;
            this.CreatedStamp = createdStamp;
            this.Scene = scene;
            this.Code = code;
            this.OperatorCredential = operatorCredential as BaseCredential;
            this.ReferenceData = data;
            this.Hint = hint;
        }

        #endregion
    }
}


