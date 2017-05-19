using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beyova;
using Beyova.ApiTracking;
using Beyova.ExceptionSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Beyova.Elastic
{
    /// <summary>
    /// Class ElasticApiEventWorkObject.
    /// </summary>
    public class ElasticApiEventWorkObject : ElasticWorkObject<ApiEventLog, LookupService>
    {
        /// <summary>
        /// Preprocesses this instance.
        /// </summary>
        public override void Preprocess()
        {
            if (this.RawData != null && this.ProcessFactor != null && !string.IsNullOrWhiteSpace(this.RawData.IpAddress))
            {
                try
                {
                    var geoInfo = this.ProcessFactor.getLocation(this.RawData.IpAddress);
                    if (geoInfo != null)
                    {
                        this.RawData.GeoInfo = new Beyova.GeoInfoBase()
                        {
                            IsoCode = geoInfo.countryCode,
                            Latitude = (decimal)geoInfo.latitude,
                            Longitude = (decimal)geoInfo.longitude,
                            CityName = geoInfo.city,
                            CountryName = geoInfo.countryName
                        };
                    }
                }
                catch { }
            }
        }
    }
}
