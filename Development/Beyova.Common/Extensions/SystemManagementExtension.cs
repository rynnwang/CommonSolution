using System.Collections.Generic;
using Beyova.Model;
using System.Management;
using System;
using System.Diagnostics;
using Beyova;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// Class SystemManagementExtension.
    /// </summary>
    public static class SystemManagementExtension
    {
        /// <summary>
        /// Gets the memory usage. (Unit: byte)
        /// </summary>
        /// <returns>System.Int64.</returns>
        public static long? GetMemoryUsage()
        {
            try
            {
                var currentProcess = Process.GetCurrentProcess();

                return currentProcess.WorkingSet64;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        /// <summary>
        /// Gets the cpu usage.
        /// </summary>
        /// <returns>System.Nullable&lt;System.Double&gt;.</returns>
        public static double? GetCpuUsage()
        {
            try
            {
                var search = new ManagementObjectSearcher("Select LoadPercentage from Win32_Processor");
                foreach (var one in search.Get())
                {
                    return one["LoadPercentage"].SafeToString().ObjectToNullableDouble();
                }
            }
            catch { }

            return null;
        }

        //public static List<string> GetOperatiingSystem()
        //{
        //    List<string> result = new List<string>();

        //    try
        //    {
        //        var search = new ManagementObjectSearcher("Select * from Win32_OperatingSystem");
        //        foreach (ManagementObject info in search.Get())
        //        {
        //            result.Add(info["LoadPercentage"].ToString().ToDouble(0));
        //        }
        //    }
        //    catch { }

        //    return result;
        //}
    }
}
