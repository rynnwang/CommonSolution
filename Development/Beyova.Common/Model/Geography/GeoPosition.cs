namespace Beyova
{
    /// <summary>
    /// Class GeoPosition.
    /// </summary>
    public struct GeoPosition
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public GeoCoordinate Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public GeoCoordinate Longitude { get; set; }

        /// <summary>
        /// To the decimal string. {Latitude, Longitude}
        /// </summary>
        /// <returns>System.String.</returns>
        public string ToDecimalString()
        {
            return string.Format("{0}, {1}", this.Latitude.ToDecimal(), this.Longitude.ToDecimal());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}", this.Latitude.InternalToString("N", "S"), this.Longitude.InternalToString("E", "W"));
        }
    }
}