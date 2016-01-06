using System.Collections.Generic;
using Beyova.Model;
using System.Management;
using System;
using System.Diagnostics;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// Class SystemManagementExtension.
    /// </summary>
    public static class SystemManagementExtension
    {
        /// <summary>
        /// Gets the machine health.
        /// </summary>
        /// <returns>MachineHealth.</returns>
        public static MachineHealth GetMachineHealth()
        {
            return new MachineHealth
            {
                MemoryUsage = GetPhysicalMemoryUsage(),
                CpuUsage = GetCpuUsage(),
                DiskUsages = GetDiskUsages(),
                TotalMemory = GetPhysicalTotalMemory(),
                ServerName = EnvironmentCore.ServerName
            };
        }

        /// <summary>
        /// Gets the memory usage. (Unit: byte)
        /// </summary>
        /// <returns>System.Int64.</returns>
        public static long? GetProcessMemoryUsage()
        {
            try
            {
                return Process.GetCurrentProcess()?.WorkingSet64;
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Gets the physical memory usage.
        /// </summary>
        /// <returns>System.Nullable&lt;System.Int64&gt;.</returns>
        public static long? GetPhysicalMemoryUsage()
        {
            try
            {
                long result = 0;
                var search = new ManagementObjectSearcher("Select AvailableMBytes from Win32_PerfRawData_PerfOS_Memory");
                foreach (ManagementObject info in search.Get())
                {
                    result += (info["AvailableMBytes"].ToString()).ToInt64();
                }

                return result;
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Gets the physical total memory.
        /// </summary>
        /// <returns>System.Nullable&lt;System.Int64&gt;.</returns>
        public static long? GetPhysicalTotalMemory()
        {
            try
            {
                long result = 0;
                ManagementObjectSearcher search = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMemory");
                foreach (ManagementObject info in search.Get())
                {
                    result += info["Capacity"].ToString().ToInt64();
                }

                return result;
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Gets the disk usages.
        /// </summary>
        /// <returns>List&lt;DiskDriveInfo&gt;.</returns>
        public static List<DiskDriveInfo> GetDiskUsages()
        {
            List<DiskDriveInfo> result = new List<DiskDriveInfo>();

            try
            {
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    result.Add(new DiskDriveInfo
                    {
                        VolumeLabel = drive.VolumeLabel,
                        IsReady = drive.IsReady,
                        Name = drive.Name,
                        TotalFreeSpace = drive.TotalFreeSpace,
                        TotalSize = drive.TotalSize
                    });
                }
            }
            catch { }

            return result;
        }

        /// <summary>
        /// Gets the gc memory.
        /// </summary>
        /// <param name="forceFullCollection">if set to <c>true</c> [force full collection].</param>
        /// <returns>System.Nullable&lt;System.Int64&gt;.</returns>
        public static long? GetGCMemory(bool forceFullCollection = false)
        {
            try
            {
                return GC.GetTotalMemory(true);
            }
            catch { }

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
