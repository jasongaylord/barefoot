using System;
using Barefoot.Models;

namespace Barefoot.Core
{
    public class Distance
    {
        private const double EarthRadiusMiles = 3956.0;

        public static double Calculate(double sLatitude,double sLongitude, double eLatitude, double eLongitude)
        {
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

        public static double Calculate(Coordinate lastCoordinate, Coordinate currentCoordinate)
        {
            return Calculate(lastCoordinate.Latitude, lastCoordinate.Longitude, currentCoordinate.Latitude,
                             currentCoordinate.Longitude);
        }

    }
}
