using System;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class UserPreference.
    /// </summary>
    public class UserPreference : SimpleBaseObject
    {
        /// <summary>
        /// Gets or sets the realm. Realm can be used based on App, Application, Product, etc.
        /// </summary>
        /// <value>The realm.</value>
        public string Realm { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets or sets the owner key.
        /// </summary>
        /// <value>The owner key.</value>
        public virtual Guid? OwnerKey { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPreference"/> class.
        /// </summary>
        public UserPreference() : base() { }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        public string SetValue(object obj)
        {
            Value = obj.ToJson();
            return Value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>System.Object.</returns>
        public object GetValue(Type type)
        {
            return (string.IsNullOrWhiteSpace(Value) || type == null) ? null : JToken.Parse(Value).ToObject(type);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T.</returns>
        public T GetValue<T>()
        {
            return string.IsNullOrWhiteSpace(Value) ? default(T) : JToken.Parse(Value).ToObject<T>();
        }
    }
}
