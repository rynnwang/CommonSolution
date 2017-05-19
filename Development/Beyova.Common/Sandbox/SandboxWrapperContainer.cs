using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class SandboxWrapperContainer.
    /// </summary>
    public static class SandboxWrapperContainer
    {
        /// <summary>
        /// The wrappers
        /// </summary>
        private static Dictionary<Type, Dictionary<Guid, SandboxWrapper>> wrappers = new Dictionary<Type, Dictionary<Guid, SandboxWrapper>>();

        /// <summary>
        /// The locker
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// Registers the specified wrapper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wrapper">The wrapper.</param>
        internal static void Register<T>(SandboxWrapper<T> wrapper)
        {
            if (wrapper != null)
            {
                var container = wrappers.GetOrCreate(typeof(T), new Dictionary<Guid, SandboxWrapper>());
                container.Merge(wrapper.Key, wrapper);
            }
        }

        /// <summary>
        /// Gets the sandbox wrapper.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>SandboxWrapper&lt;T&gt;.</returns>
        public static SandboxWrapper<T> GetSandboxWrapper<T>(Guid? key)
        {
            SandboxWrapper wrapper;
            Dictionary<Guid, SandboxWrapper> container;

            return
                (key.HasValue && wrappers.TryGetValue(typeof(T), out container) && container.TryGetValue(key.Value, out wrapper)) ? (SandboxWrapper<T>)wrapper : null;
        }
    }
}
