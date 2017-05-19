/*
 * Location.cs
 *
 * Copyright (C) 2008 MaxMind Inc.  All Rights Reserved.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */


using System;
using System.IO;

/// <summary>
/// Class Location.
/// </summary>
public class Location
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
    /// The city
    /// </summary>
    public String city;
    /// <summary>
    /// The postal code
    /// </summary>
    public String postalCode;
    /// <summary>
    /// The latitude
    /// </summary>
    public double latitude;
    /// <summary>
    /// The longitude
    /// </summary>
    public double longitude;
    /// <summary>
    /// The dma_code
    /// </summary>
    public int dma_code;
    /// <summary>
    /// The area_code
    /// </summary>
    public int area_code;
    /// <summary>
    /// The region name
    /// </summary>
    public String regionName;
    /// <summary>
    /// The metro_code
    /// </summary>
    public int metro_code;

    /// <summary>
    /// The eart h_ diameter
    /// </summary>
    private static double EARTH_DIAMETER = 2 * 6378.2;
    /// <summary>
    /// The pi
    /// </summary>
    private static double PI = 3.14159265;
    /// <summary>
    /// The ra d_ convert
    /// </summary>
    private static double RAD_CONVERT = PI / 180;

    /// <summary>
    /// Distances the specified loc.
    /// </summary>
    /// <param name="loc">The loc.</param>
    /// <returns>System.Double.</returns>
    public double distance(Location loc)
    {
        double delta_lat, delta_lon;
        double temp;

        double lat1 = latitude;
        double lon1 = longitude;
        double lat2 = loc.latitude;
        double lon2 = loc.longitude;

        // convert degrees to radians
        lat1 *= RAD_CONVERT;
        lat2 *= RAD_CONVERT;

        // find the deltas
        delta_lat = lat2 - lat1;
        delta_lon = (lon2 - lon1) * RAD_CONVERT;

        // Find the great circle distance
        temp = Math.Pow(Math.Sin(delta_lat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(delta_lon / 2), 2);
        return EARTH_DIAMETER * Math.Atan2(Math.Sqrt(temp), Math.Sqrt(1 - temp));
    }
}

