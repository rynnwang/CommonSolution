using System;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova
{
    internal static class FeatureModuleSwitch
    {
        /// <summary>
        /// The switch objects
        /// </summary>
        private static Dictionary<string, FeatureModuleSwitchObject> switchObjects = new Dictionary<string, FeatureModuleSwitchObject>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The locker
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// Registers the module.
        /// </summary>
        internal static void RegisterModule(string moduleName)
        {
            if (!string.IsNullOrWhiteSpace(moduleName))
            {
                lock (locker)
                {
                    switchObjects.Merge(moduleName, new FeatureModuleSwitchObject
                    {
                        IsEnabled = true,
                        ModuleName = moduleName
                    }, false);
                }
            }
        }

        /// <summary>
        /// Changes the status.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="isEnabled">The is enabled.</param>
        /// <param name="until">The until.</param>
        internal static void ChangeStatus(string moduleName, bool isEnabled, DateTime? until = null)
        {
            ChangeStatus(new FeatureModuleSwitchObject
            {
                ModuleName = moduleName,
                IsEnabled = isEnabled,
                KeepStatusUntilStamp = until
            });
        }

        /// <summary>
        /// Changes the status.
        /// </summary>
        /// <param name="switchObject">The switch object.</param>
        internal static void ChangeStatus(FeatureModuleSwitchObject switchObject)
        {
            try
            {
                switchObject.CheckNullObject(nameof(switchObject));
                switchObject.ModuleName.CheckEmptyString(nameof(switchObject.ModuleName));

                var hitSwitchObject = switchObjects.TryGetValue(switchObject.ModuleName);
                hitSwitchObject?.SetStatus(switchObject.IsEnabled, switchObject.KeepStatusUntilStamp);
            }
            catch (Exception ex)
            {
                throw ex.Handle(switchObject);
            }
        }

        /// <summary>
        /// Changes the status.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        /// <param name="durationInSecond">The duration in second.</param>
        internal static void ChangeStatus(string moduleName, bool isEnabled, long durationInSecond)
        {
            ChangeStatus(moduleName, isEnabled, DateTime.UtcNow.AddSeconds(durationInSecond));
        }

        /// <summary>
        /// Checks the module availability.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <exception cref="Beyova.ExceptionSystem.ServiceUnavailableException">FeatureSwitch</exception>
        internal static void CheckModuleAvailability(string moduleName)
        {
            if (!string.IsNullOrWhiteSpace(moduleName))
            {
                var switchObject = switchObjects.TryGetValue(moduleName);
                if (switchObject != null)
                {
                    switchObject.EnsureStatus();

                    if (switchObject.IsEnabled)
                    {
                        return;
                    }
                }
            }
            else
            {
                return;
            }

            throw new ServiceUnavailableException(moduleName, "FeatureSwitch");
        }

        /// <summary>
        /// Gets the module work status.
        /// </summary>
        /// <returns>Dictionary&lt;System.String, System.Boolean&gt;.</returns>
        public static Dictionary<string, bool> GetModuleWorkStatus()
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

            lock (locker)
            {
                foreach (var one in switchObjects)
                {
                    result.Add(one.Key, one.Value.IsEnabled);
                }
            }

            return result;
        }
    }
}