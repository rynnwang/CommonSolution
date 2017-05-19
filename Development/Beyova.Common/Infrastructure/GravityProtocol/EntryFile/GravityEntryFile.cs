using System;
using System.IO;
using System.Text;
using Beyova.Gravity;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class GravityEntryFile.
    /// </summary>
    class GravityEntryFile : GravityEntryObject
    {
        /// <summary>
        /// Gets or sets the issued stamp.
        /// </summary>
        /// <value>The issued stamp.</value>
        [JsonRequired]
        public DateTime? IssuedStamp { get; set; }

        /// <summary>
        /// Gets or sets the issued to.
        /// </summary>
        /// <value>The issued to.</value>
        [JsonRequired]
        public string IssuedTo { get; set; }

        /// <summary>
        /// Loads the specified path.
        /// </summary>
        /// <param name="path">The path. If path is null or empty, use {application base directory}/gravity.gef as default.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>BeyovaLicenseFile.</returns>
        public static GravityEntryFile Load(string path = null, bool throwException = false)
        {
            try
            {
                path = path.SafeToString(Path.Combine(EnvironmentCore.ApplicationBaseDirectory, "gravity.gef"));

                if (!File.Exists(path))
                {
                    throw new FileNotFoundException(string.Format("{0} is not found.", path));
                }

                return JsonConvert.DeserializeObject<GravityEntryFile>(Encoding.UTF8.GetString(File.ReadAllBytes(path)).DecryptR3DES());
            }
            catch (Exception ex)
            {
                var baseException = ex.Handle(new { path });
                if (throwException)
                {
                    throw baseException;
                }
                else
                {
                    Framework.ApiTracking?.LogException(baseException.ToExceptionInfo());
                    return null;
                }
            }
        }
    }
}
