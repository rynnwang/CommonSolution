/*
 * DatabaseInfo.cs
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
/// Class DatabaseInfo.
/// </summary>
public class DatabaseInfo
{
    /// <summary>
    /// The countr y_ edition
    /// </summary>
    public static int COUNTRY_EDITION = 1;
    /// <summary>
    /// The regio n_ editio n_ re v0
    /// </summary>
    public static int REGION_EDITION_REV0 = 7;
    /// <summary>
    /// The regio n_ editio n_ re v1
    /// </summary>
    public static int REGION_EDITION_REV1 = 3;
    /// <summary>
    /// The cit y_ editio n_ re v0
    /// </summary>
    public static int CITY_EDITION_REV0 = 6;
    /// <summary>
    /// The cit y_ editio n_ re v1
    /// </summary>
    public static int CITY_EDITION_REV1 = 2;
    /// <summary>
    /// The or g_ edition
    /// </summary>
    public static int ORG_EDITION = 5;
    /// <summary>
    /// The is p_ edition
    /// </summary>
    public static int ISP_EDITION = 4;
    /// <summary>
    /// The prox y_ edition
    /// </summary>
    public static int PROXY_EDITION = 8;
    /// <summary>
    /// The asnu m_ edition
    /// </summary>
    public static int ASNUM_EDITION = 9;
    /// <summary>
    /// The netspee d_ edition
    /// </summary>
    public static int NETSPEED_EDITION = 10;
    /// <summary>
    /// The domai n_ edition
    /// </summary>
    public static int DOMAIN_EDITION = 11;
    /// <summary>
    /// The countr y_ editio n_ v6
    /// </summary>
    public static int COUNTRY_EDITION_V6 = 12;
    /// <summary>
    /// The asnu m_ editio n_ v6
    /// </summary>
    public static int ASNUM_EDITION_V6 = 21;
    /// <summary>
    /// The is p_ editio n_ v6
    /// </summary>
    public static int ISP_EDITION_V6 = 22;
    /// <summary>
    /// The or g_ editio n_ v6
    /// </summary>
    public static int ORG_EDITION_V6 = 23;
    /// <summary>
    /// The domai n_ editio n_ v6
    /// </summary>
    public static int DOMAIN_EDITION_V6 = 24;
    /// <summary>
    /// The cit y_ editio n_ re V1_ v6
    /// </summary>
    public static int CITY_EDITION_REV1_V6 = 30;
    /// <summary>
    /// The cit y_ editio n_ re V0_ v6
    /// </summary>
    public static int CITY_EDITION_REV0_V6 = 31;
    /// <summary>
    /// The netspee d_ editio n_ re v1
    /// </summary>
    public static int NETSPEED_EDITION_REV1 = 32;
    /// <summary>
    /// The netspee d_ editio n_ re V1_ v6
    /// </summary>
    public static int NETSPEED_EDITION_REV1_V6 = 33;


    //private static SimpleDateFormat formatter = new SimpleDateFormat("yyyyMMdd");

    /// <summary>
    /// The information
    /// </summary>
    private String info;
    /**
      * Creates a new DatabaseInfo object given the database info String.
      * @param info
      */

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseInfo"/> class.
    /// </summary>
    /// <param name="info">The information.</param>
    public DatabaseInfo(String info)
    {
        this.info = info;
    }

    /// <summary>
    /// Gets the type.
    /// </summary>
    /// <returns>System.Int32.</returns>
    public int getType()
    {
        if ((info == null) | (info == ""))
        {
            return COUNTRY_EDITION;
        }
        else
        {
            // Get the type code from the database info string and then
            // subtract 105 from the value to preserve compatability with
            // databases from April 2003 and earlier.
            return Convert.ToInt32(info.Substring(4, 3)) - 105;
        }
    }

    /**
     * Returns true if the database is the premium version.
     *
     * @return true if the premium version of the database.
     */
    /// <summary>
    /// Determines whether this instance is premium.
    /// </summary>
    /// <returns><c>true</c> if this instance is premium; otherwise, <c>false</c>.</returns>
    public bool isPremium()
    {
        return info.IndexOf("FREE") < 0;
    }

    /**
     * Returns the date of the database.
     *
     * @return the date of the database.
     */
    /// <summary>
    /// Gets the date.
    /// </summary>
    /// <returns>DateTime.</returns>
    public DateTime getDate()
    {
        for (int i = 0; i < info.Length - 9; i++)
        {
            if (Char.IsWhiteSpace(info[i]) == true)
            {
                String dateString = info.Substring(i + 1, 8);
                try
                {
                    //synchronized (formatter) {
                    return DateTime.ParseExact(dateString, "yyyyMMdd", null);
                    //}
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
                break;
            }
        }
        return DateTime.Now;
    }

    /// <summary>
    /// To the string.
    /// </summary>
    /// <returns>String.</returns>
    public String toString()
    {
        return info;
    }
}


