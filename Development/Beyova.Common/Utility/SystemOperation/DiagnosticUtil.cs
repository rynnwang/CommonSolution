using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    /// Class DiagnosticUtil.
    /// </summary>
    public static class DiagnosticUtil
    {
        /// <summary>
        /// Diagnostics the specified base URL.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="paths">The paths.</param>
        /// <returns></returns>
        public static Dictionary<string, bool> Diagnostic(string baseUrl, params string[] paths)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();

            if (!string.IsNullOrWhiteSpace(baseUrl) && paths.HasItem())
            {
                Parallel.ForEach(paths, (path) =>
                {
                    var httpRequest = string.Format("{0}/{1}", baseUrl.TrimEnd(' ', '/'), path.TrimStart('/', ' ')).CreateHttpWebRequest();

                    try
                    {
                        var response = httpRequest.GetResponse();
                        var length = response.ContentLength;

                        result.Merge(httpRequest.RequestUri.ToString(), true);
                    }
                    catch
                    {
                        result.Merge(httpRequest.RequestUri.ToString(), false);
                    }
                });
            }

            return result;
        }

        #region Ping

        // http://referencesource.microsoft.com/#System/net/System/Net/NetworkInformation/ping.cs,45144e953597ec31
        private const int defaultSendBufferSize = 32;  //As same as ping.exe

        // http://referencesource.microsoft.com/#System/net/System/Net/NetworkInformation/ping.cs,dfb5d81e2df261e3
        private static int defaultTimeOut = 5000; // 5 seconds same as ping.exe

        /// <summary>
        /// Pings the specified host name or address.
        /// </summary>
        /// <param name="hostNameOrAddress">The host name or address.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="options">The options.</param>
        /// <returns>System.Net.NetworkInformation.PingReply.</returns>
        public static PingReply Ping(string hostNameOrAddress, int? timeout = null, byte[] buffer = null, PingOptions options = null)
        {
            try
            {
                hostNameOrAddress.CheckEmptyString("hostNameOrAddress");

                var ping = new Ping();
                return ping.Send(hostNameOrAddress, timeout ?? defaultTimeOut, buffer ?? new byte[defaultSendBufferSize], options);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { hostNameOrAddress, timeout, buffer, options });
            }
        }

        #endregion
    }
}
