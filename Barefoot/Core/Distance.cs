using System;
using System.Device.Location;
using Barefoot.Models;

namespace Barefoot.Core
{
    public class Distance
    {
        private const double EarthRadiusMiles = 3956.0;

        public static double Calculate(double sLatitude,double sLongitude, double eLatitude, double eLongitude)
        {

            ////var coord1 = new GeoCoordinate(sLatitude, sLongitude);
            ////var coord2 = new GeoCoordinate(eLatitude, eLongitude);
            ////var meters = coord1.GetDistanceTo(coord2);
            /// 
            /// 1 meter = 0.00062137119 miles
            /// http://www.dreamincode.net/code/snippet2414.htm

            var sLatitudeRadians = sLatitude * (Math.PI / 180.0);
            var sLongitudeRadians = sLongitude * (Math.PI / 180.0);
            var eLatitudeRadians = eLatitude * (Math.PI / 180.0);
            var eLongitudeRadians = eLongitude * (Math.PI / 180.0);

            var dLongitude = eLongitudeRadians - sLongitudeRadians;
            var dLatitude = eLatitudeRadians - sLatitudeRadians;

            var result1 = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) + Math.Cos(sLatitudeRadians) * Math.Cos(eLatitudeRadians) * Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);

            var result2 = 2.0 * Math.Atan2(Math.Sqrt(result1), Math.Sqrt(1.0 - result1));

            return (EarthRadiusMiles * result2);
        }

        ////// Javascript function to calculate
        ////var R = 6371; // Radius of the earth in km
        ////var dLat = (lat2 - lat1).toRad();  // Javascript functions in radians
        ////var dLon = (lon2 - lon1).toRad();
        ////var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
        ////        Math.cos(lat1.toRad()) * Math.cos(lat2.toRad()) *
        ////        Math.sin(dLon / 2) * Math.sin(dLon / 2);
        ////var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        ////var d = R * c; // Distance in km


        public static double Calculate(Coordinate lastCoordinate, Coordinate currentCoordinate)
        {
            return Calculate(lastCoordinate.Latitude, lastCoordinate.Longitude, currentCoordinate.Latitude,
                             currentCoordinate.Longitude);
        }

    }
}
