using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Web;
using ifunction.Model;

namespace ifunction
{
    /// <summary>
    /// Class EnvironmentCore.
    /// </summary>
    public static class EnvironmentCore
    {
        /// <summary>
        /// The application base directory
        /// </summary>
        public static readonly string ApplicationBaseDirectory;

        /// <summary>
        /// The log directory
        /// </summary>
        public static readonly string LogDirectory;

        /// <summary>
        /// The application identifier
        /// </summary>
        public static readonly int ApplicationId;

        /// <summary>
        /// The server name
        /// </summary>
        public static readonly string ServerName;

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public static readonly string ApplicationName;

        /// <summary>
        /// Initializes static members of the <see cref="EnvironmentCore"/> class.
        /// </summary>
        static EnvironmentCore()
        {
            ApplicationBaseDirectory = !string.IsNullOrWhiteSpace(HttpRuntime.AppDomainAppVirtualPath)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin")
                : AppDomain.CurrentDomain.BaseDirectory;

            LogDirectory = Path.Combine(ApplicationBaseDirectory, "logs");
            ApplicationId = System.AppDomain.CurrentDomain.Id;

            try
            {
                ServerName = Environment.MachineName;
            }
            catch { ServerName = string.Empty; }

            try
            {
                if (AppDomain.CurrentDomain != null)
                {
                    ApplicationName = AppDomain.CurrentDomain.FriendlyName;
                }

                if (string.IsNullOrWhiteSpace(ApplicationName))
                {
                    ApplicationName = Assembly.GetEntryAssembly().FullName;
                }
            }
            catch { ApplicationName = string.Empty; }
        }

        /// <summary>
        /// Gets the total memory. Unit: bytes
        /// </summary>
        /// <value>The total memory.</value>
        public static long TotalMemory
        {
            get
            {
                return GC.GetTotalMemory(false);
            }
        }
    }
}
