using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    /// Class GeoInfoBase.
    /// </summary>
    public class GeoInfoBase
    {
        /// <summary>
        /// Gets or sets the iso code.
        /// </summary>
        /// <value>The iso code.</value>
        public string IsoCode { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }
    }
}
