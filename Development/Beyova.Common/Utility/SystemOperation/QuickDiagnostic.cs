using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    /// </summary>
    public static class QuickDiagnostic
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
    }
}
