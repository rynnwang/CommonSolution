using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.WebExtension.HttpLongPolling
{
    /// <summary>
    /// Class PollingDictionary.
    /// </summary>
    /// <typeparam name="TObject">The type of the t object.</typeparam>
    public sealed class PollingDictionary<TObject>
    {
        /// <summary>
        /// The dictionary
        /// </summary>
        ConcurrentDictionary<string, ConcurrentDictionary<string, TObject>> dictionary = new ConcurrentDictionary<string, ConcurrentDictionary<string, TObject>>();

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<string> Keys
        {
            get
            {
                return this.dictionary.Keys;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PollingDictionary{TObject}"/> class.
        /// </summary>
        public PollingDictionary()
        {
        }

        /// <summary>
        /// Creates the or replace object.
        /// </summary>
        /// <param name="fullIdentifier">The full identifier.</param>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if operation is done by create, <c>false</c> otherwise.</returns>
        public bool CreateOrReplaceObject(string fullIdentifier, TObject obj)
        {
            bool result = false;

            if (!string.IsNullOrWhiteSpace(fullIdentifier))
            {
                result = true;

                string majorIdentifier, resourceIdentifier;
                majorIdentifier = SplitIdentifier(fullIdentifier, out resourceIdentifier);

                var userDictionary = this.dictionary.GetOrAdd(majorIdentifier, new ConcurrentDictionary<string, TObject>());

                userDictionary.AddOrUpdate(resourceIdentifier, obj, (k, oldValue) =>
                {
                    result = false;
                    return obj;
                });
            }

            return result;
        }

        /// <summary>
        /// Tries the get objects by user identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>Dictionary{System.String`0}.</returns>
        public Dictionary<string, TObject> GetObjectsByMajorIdentifier(string identifier)
        {
            string resourceIdentifier;
            var majorIdentifier = SplitIdentifier(identifier, out  resourceIdentifier);

            Dictionary<string, TObject> result = new Dictionary<string, TObject>();

            if (!string.IsNullOrWhiteSpace(majorIdentifier))
            {
                ConcurrentDictionary<string, TObject> container;

                if (this.dictionary.TryGetValue(majorIdentifier, out container))
                {
                    foreach (var key in container.Keys)
                    {
                        result.Add(CombineFullIdentifier(majorIdentifier, key), container[key]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the object by full identifier.
        /// </summary>
        /// <param name="majorIdentifier">The major identifier.</param>
        /// <param name="resourceIdentifier">The resource identifier.</param>
        /// <returns>`0.</returns>
        public TObject GetObjectByFullIdentifier(string majorIdentifier, string resourceIdentifier)
        {
            TObject result = default(TObject);

            if (!string.IsNullOrWhiteSpace(majorIdentifier))
            {
                ConcurrentDictionary<string, TObject> container;
                if (this.dictionary.TryGetValue(majorIdentifier, out container))
                {
                    container.TryGetValue(resourceIdentifier.SafeToString(), out result);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the object by full identifier.
        /// </summary>
        /// <param name="fullIdentifier">The full identifier.</param>
        /// <returns>`0.</returns>
        public TObject GetObjectByFullIdentifier(string fullIdentifier)
        {
            TObject result = default(TObject);

            if (!string.IsNullOrWhiteSpace(fullIdentifier))
            {
                string majorIdentifier, resourceIdentifier;
                majorIdentifier = SplitIdentifier(fullIdentifier, out resourceIdentifier);

                result = GetObjectByFullIdentifier(majorIdentifier, resourceIdentifier);
            }

            return result;
        }

        /// <summary>
        /// Removes the object.
        /// </summary>
        /// <param name="fullIdentifier">The full identifier.</param>
        /// <returns>`0.</returns>
        public TObject RemoveObject(string fullIdentifier)
        {
            TObject result = default(TObject);

            if (!string.IsNullOrWhiteSpace(fullIdentifier))
            {
                string majorIdentifier, resourceIdentifier;
                majorIdentifier = SplitIdentifier(fullIdentifier, out resourceIdentifier);

                ConcurrentDictionary<string, TObject> container;
                if (this.dictionary.TryGetValue(majorIdentifier, out container))
                {
                    container.TryRemove(resourceIdentifier, out result);
                }
            }

            return result;
        }

        /// <summary>
        /// Splits the identifier.
        /// </summary>
        /// <param name="fullIdentifier">The full identifier.</param>
        /// <param name="resourceIdentifier">The resource identifier.</param>
        /// <returns>System.String.</returns>
        public static string SplitIdentifier(string fullIdentifier, out string resourceIdentifier)
        {
            string majorIdentifier = string.Empty;
            resourceIdentifier = string.Empty;

            if (!string.IsNullOrWhiteSpace(fullIdentifier))
            {
                var parts = fullIdentifier.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                majorIdentifier = parts.Length > 0 ? parts[0] : string.Empty;
                resourceIdentifier = parts.Length > 1 ? parts[1] : string.Empty;
            }

            return majorIdentifier;
        }

        /// <summary>
        /// Combines the full identifier.
        /// </summary>
        /// <param name="majorIdentifier">The major identifier.</param>
        /// <param name="resourceIdentifier">The resource identifier.</param>
        /// <returns>System.String.</returns>
        public static string CombineFullIdentifier(string majorIdentifier, string resourceIdentifier)
        {
            return string.Format("{0}/{1}", majorIdentifier, resourceIdentifier).Trim('/');
        }
    }
}
