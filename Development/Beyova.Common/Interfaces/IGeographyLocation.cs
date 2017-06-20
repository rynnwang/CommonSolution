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
        decimal? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        decimal? Longitude { get; set; }
    }
}