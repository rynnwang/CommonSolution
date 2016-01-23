using System;

namespace Beyova
{
    /// <summary>
    /// Interface IGeographyLocation
    /// </summary>
    public interface IGeographyLocation
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        double? Longitude { get; set; }
    }
}
