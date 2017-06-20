using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Beyova;
using Beyova.ProgrammingIntelligence;

namespace Beyova
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
        /// The local machine host name
        /// </summary>
        public static readonly string LocalMachineHostName = string.Empty;

        /// <summary>
        /// The local machine ip address
        /// </summary>
        public static readonly string LocalMachineIpAddress = string.Empty;

        /// <summary>
        /// Gets the name of the project.
        /// </summary>
        /// <value>The name of the project.</value>
        public static string ProjectName { get; private set; }

        /// <summary>
        /// The descending assembly dependency chain. Descending here means order by referenced amount.
        /// So result would be like: web -> core -> contract - > common -> json.net, etc.
        /// </summary>
        internal static List<Assembly> DescendingAssemblyDependencyChain;

        /// <summary>
        /// The ascending assembly dependency chain
        /// </summary>
        internal static List<Assembly> AscendingAssemblyDependencyChain;

        /// <summary>
        /// Gets the entry assembly.
        /// </summary>
        /// <value>
        /// The entry assembly.
        /// </value>
        public static Assembly EntryAssembly { get { return DescendingAssemblyDependencyChain.FirstOrDefault(); } }

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

            DescendingAssemblyDependencyChain = ReflectionExtension.GetAppDomainAssemblies().GetAssemblyDependencyChain(true);
            AscendingAssemblyDependencyChain = new List<Assembly>(DescendingAssemblyDependencyChain);
            DescendingAssemblyDependencyChain.Reverse();

            try
            {
                ServerName = Environment.MachineName;
            }
            catch { ServerName = string.Empty; }

            try
            {
                LocalMachineHostName = Dns.GetHostName();

                var host = Dns.GetHostEntry(LocalMachineHostName);
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        LocalMachineIpAddress = ip.ToString();
                        break;
                    }
                }
            }
            catch { }

            try
            {
                if (AppDomain.CurrentDomain != null)
                {
                    ProjectName = AppDomain.CurrentDomain.FriendlyName;
                }

                if (string.IsNullOrWhiteSpace(ProjectName) || ProjectName.IndexOfAny(new char[] { '/', '\\', ':', '*' }) > -1)
                {
                    ProjectName = Assembly.GetEntryAssembly()?.FullName;
                }

                if (string.IsNullOrWhiteSpace(ProjectName))
                {
                    ProjectName = FindProjectName();
                }
            }
            catch { ProjectName = string.Empty; }
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

        /// <summary>
        /// Finds the name of the project.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string FindProjectName()
        {
            try
            {
                string result = null;
                foreach (var one in DescendingAssemblyDependencyChain)
                {
                    var component = one.GetCustomAttribute<BeyovaComponentAttribute>();
                    if (component != null && !string.IsNullOrWhiteSpace(component.Id))
                    {
                        result = component.Id;
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(result))
                {
                    result = DescendingAssemblyDependencyChain.FirstOrDefault()?.GetName()?.Name;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="defaultSubDirectoryName">Default name of the sub directory.</param>
        /// <returns>System.IO.DirectoryInfo.</returns>
        internal static DirectoryInfo GetDirectory(string directory, string defaultSubDirectoryName = null)
        {
            string directoryPath;
            if (string.IsNullOrWhiteSpace(directory))
            {
                directoryPath = string.IsNullOrWhiteSpace(defaultSubDirectoryName) ? EnvironmentCore.ApplicationBaseDirectory : Path.Combine(EnvironmentCore.ApplicationBaseDirectory, defaultSubDirectoryName);
            }
            else if (directory.Contains(':') || directory.StartsWith("\\\\"))
            {
                directoryPath = directory;
            }
            else
            {
                directoryPath = directory.TrimStart('/', '~', '.').TrimEnd('/', '\\').Replace('/', '\\');
                directoryPath = string.IsNullOrWhiteSpace(directoryPath) ? EnvironmentCore.ApplicationBaseDirectory : Path.Combine(EnvironmentCore.ApplicationBaseDirectory, directoryPath);
            }

            return new DirectoryInfo(directoryPath);
        }

        /// <summary>
        /// Gets the assembly hash.
        /// </summary>
        /// <returns>System.String.</returns>
        internal static string GetAssemblyHash()
        {
            try
            {
                StringBuilder builder = new StringBuilder(DescendingAssemblyDependencyChain.Count * 16);
                MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();

                foreach (var one in DescendingAssemblyDependencyChain)
                {
                    builder.Append(md5Provider.ComputeHash(File.ReadAllBytes(one.Location)).EncodeBase64());
                }

                return md5Provider.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString())).EncodeBase64();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}