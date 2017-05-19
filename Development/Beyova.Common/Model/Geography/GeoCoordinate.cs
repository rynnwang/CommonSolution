using System;

namespace Beyova
{
    /// <summary>
    /// Class GeoCoordinate.
    /// </summary>
    public struct GeoCoordinate
    {
        /// <summary>
        /// Gets or sets the degrees.
        /// </summary>
        /// <value>The degrees.</value>
        public int Degrees { get; set; }

        /// <summary>
        /// Gets or sets the minutes.
        /// </summary>
        /// <value>The minutes.</value>
        public int Minutes { get; set; }

        /// <summary>
        /// Gets or sets the seconds.
        /// </summary>
        /// <value>The seconds.</value>
        public double Seconds { get; set; }

        /// <summary>
        /// To the decimal.
        /// </summary>
        /// <returns>System.Decimal.</returns>
        internal decimal ToDecimal()
        {
            return (int)Degrees + ((decimal)Minutes / 60) + ((decimal)Seconds / 3600);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="positivePrefix">The positive prefix.</param>
        /// <param name="negativePrefix">The negative prefix.</param>
        /// <param name="positiveSuffix">The positive suffix.</param>
        /// <param name="negativeSuffix">The negative suffix.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        internal string InternalToString(string positivePrefix = null, string negativePrefix = null, string positiveSuffix = null, string negativeSuffix = null)
        {
            return string.Format("{0}{1}°{2}'{3}''{4}", this.Degrees > 0 ? positivePrefix : negativePrefix, this.Degrees, this.Minutes, this.Seconds, this.Degrees > 0 ? positiveSuffix : negativeSuffix);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return InternalToString();
        }

        /// <summary>
        /// Froms the decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>GeoCoordinate.</returns>
        public static GeoCoordinate FromDecimal(decimal value)
        {
            var degrees = (int)value;
            var decimalMinutes = (Math.Abs(value) - Math.Abs(degrees)) * 60;
            var minutes = (int)decimalMinutes;
            var seconds = (double)Math.Round((decimalMinutes - minutes) * 60);

            return new GeoCoordinate
            {
                Degrees = degrees,
                Minutes = minutes,
                Seconds = seconds
            };
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="GeoCoordinate"/> to <see cref="System.Decimal"/>.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator decimal(GeoCoordinate coordinate)
        {
            return coordinate.ToDecimal();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="GeoCoordinate"/> to <see cref="System.Double"/>.
        /// </summary>
        /// <param name="coordinate">The coordinate.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator double(GeoCoordinate coordinate)
        {
            return (double)coordinate.ToDecimal();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Decimal"/> to <see cref="GeoCoordinate"/>.
        /// </summary>
        /// <param name="decimalObject">The decimal object.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator GeoCoordinate(decimal decimalObject)
        {
            return GeoCoordinate.FromDecimal(decimalObject);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Double"/> to <see cref="GeoCoordinate"/>.
        /// </summary>
        /// <param name="doubleObject">The double object.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator GeoCoordinate(double doubleObject)
        {
            return GeoCoordinate.FromDecimal((decimal)doubleObject);
        }
    }
}
