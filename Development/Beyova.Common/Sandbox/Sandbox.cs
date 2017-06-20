using System;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// Class BaseSandbox.
    /// </summary>
    public sealed class Sandbox : BaseSandbox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSandbox" /> class.
        /// </summary>
        /// <param name="applicationDirectory">The application directory.</param>
        public Sandbox(string applicationDirectory = null)
            : this(new SandboxSetting { ApplicationBaseDirectory = applicationDirectory })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSandbox" /> class.
        /// </summary>
        /// <param name="setting">The setting.</param>
        public Sandbox(SandboxSetting setting)
            : base(setting)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSandbox" /> class.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <param name="key">The identifier.</param>
        private Sandbox(SandboxSetting setting, Guid key)
            : base(setting, key)
        {
        }

        /// <summary>
        /// Creates the anonymous sandbox.
        /// </summary>
        /// <returns>Sandbox.</returns>
        public static Sandbox CreateAnonymousSandbox()
        {
            var id = Guid.NewGuid();
            var tmpDirectory = Path.Combine(Path.GetTempPath(), id.ToString());
            var result = new Sandbox(new SandboxSetting { ApplicationBaseDirectory = tmpDirectory }, id);

            return result;
        }
    }
}