/*
 * Country.cs
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
/// Class Country.
/// </summary>
public class Country
{

    /// <summary>
    /// The code
    /// </summary>
    private String code;
    /// <summary>
    /// The name
    /// </summary>
    private String name;

    /**
     * Creates a new Country.
     *
     * @param code the country code.
     * @param name the country name.
     */
    /// <summary>
    /// Initializes a new instance of the <see cref="Country"/> class.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="name">The name.</param>
    public Country(String code, String name)
    {
        this.code = code;
        this.name = name;
    }
    /**
      * Returns the ISO two-letter country code of this country.
      *
      * @return the country code.
      */
    /// <summary>
    /// Gets the code.
    /// </summary>
    /// <returns>String.</returns>
    public String getCode()
    {
        return code;
    }

    /**
     * Returns the name of this country.
     *
     * @return the country name.
     */
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <returns>String.</returns>
    public String getName()
    {
        return name;
    }
}
