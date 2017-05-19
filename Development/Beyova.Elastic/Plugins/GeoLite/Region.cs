using System;
using System.IO;
/// <summary>
/// Class Region.
/// </summary>
public class Region
{
    /// <summary>
    /// The country code
    /// </summary>
    public String countryCode;
    /// <summary>
    /// The country name
    /// </summary>
    public String countryName;
    /// <summary>
    /// The region
    /// </summary>
    public String region;
    /// <summary>
    /// Initializes a new instance of the <see cref="Region"/> class.
    /// </summary>
    public Region()
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="Region"/> class.
    /// </summary>
    /// <param name="countryCode">The country code.</param>
    /// <param name="countryName">Name of the country.</param>
    /// <param name="region">The region.</param>
    public Region(String countryCode, String countryName, String region)
    {
        this.countryCode = countryCode;
        this.countryName = countryName;
        this.region = region;
    }
    /// <summary>
    /// Getcountries the code.
    /// </summary>
    /// <returns>String.</returns>
    public String getcountryCode()
    {
        return countryCode;
    }
    /// <summary>
    /// Getcountries the name.
    /// </summary>
    /// <returns>String.</returns>
    public String getcountryName()
    {
        return countryName;
    }
    /// <summary>
    /// Getregions this instance.
    /// </summary>
    /// <returns>String.</returns>
    public String getregion()
    {
        return region;
    }
}

