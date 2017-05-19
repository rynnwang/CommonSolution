using System;

namespace Beyova
{
    /// <summary>
    /// Class FeatureModuleSwitchObject.
    /// </summary>
    internal class FeatureModuleSwitchObject
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        public string ModuleName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the keep status until stamp.
        /// </summary>
        /// <value>The keep status until stamp.</value>
        public DateTime? KeepStatusUntilStamp { get; set; }

        /// <summary>
        /// The _locker
        /// </summary>
        private object _locker = new object();

        /// <summary>
        /// Ensures the status.
        /// </summary>
        public void EnsureStatus()
        {
            if (KeepStatusUntilStamp.HasValue && KeepStatusUntilStamp < DateTime.UtcNow)
            {
                lock (_locker)
                {
                    if (KeepStatusUntilStamp.HasValue && KeepStatusUntilStamp < DateTime.UtcNow)
                    {
                        IsEnabled = !IsEnabled;
                        KeepStatusUntilStamp = null;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the status.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        /// <param name="until">The until.</param>
        public void SetStatus(bool isEnabled, DateTime? until)
        {
            lock (_locker)
            {
                this.IsEnabled = isEnabled;
                this.KeepStatusUntilStamp = until;
            }
        }
    }
}