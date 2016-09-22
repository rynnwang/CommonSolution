using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Beyova
{
    /// <summary>
    /// Class NetworkDiagnostic.
    /// </summary>
    public static class NetworkDiagnostic
    {
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
                throw ex.Handle( new { hostNameOrAddress, timeout, buffer, options });
            }
        }

        #endregion
    }
}

