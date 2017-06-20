﻿namespace Beyova
{
    /// <summary>
    /// Class GeoInfoBase.
    /// </summary>
    public class GeoInfoBase : IGeographyLocation
    {
        /// <summary>
        /// Gets or sets the iso code.
        /// </summary>
        /// <value>The iso code.</value>
        public string IsoCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        /// <value>The name of the country.</value>
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        /// <value>The name of the city.</value>
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public decimal? Longitude { get; set; }
    }
}