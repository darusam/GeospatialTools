using System;
using System.Collections.Generic;
using System.Text;

namespace CDMSmith.GeospatialTools.Tools
{
    public static class Util
    {
        public static double DegreesToRadians(double degrees)
        {
            double radians = degrees % 360;
            return radians * Math.PI / 180;
        }

        public static double RadiansToDegrees(double radians)
        {
            double degrees = radians %  (2 * Math.PI);
            return degrees * 180 / Math.PI;
        }

        public static double Bearing(double x1, double y1, double x2, double y2)
        {
            double lon1 = DegreesToRadians(x1);
            double lon2 = DegreesToRadians(x2);
            double lat1 = DegreesToRadians(y1);
            double lat2 = DegreesToRadians(y2);

            double a = Math.Sin(lon2 - lon1) * Math.Cos(lat2);
            double b = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1);

            return RadiansToDegrees(Math.Atan2(a, b));
        }

        /* TODO: Handle units */
        public static double LengthToRadians(double distance /* , unit */)
        {
            double factor = 1; // Meters
            return distance / factor;
        }

        public static Tuple<double, double> Destination(double x, double y, double distance, double bearing )
        {
            double lon1 = DegreesToRadians(x);
            double lat1 = DegreesToRadians(y);

            double bearingRadians = DegreesToRadians(bearing);
            double radians = LengthToRadians(distance);

            double lat2 = Math.Asin(Math.Sin(lat1) * Math.Cos(radians) + Math.Cos(lat1) * Math.Sin(radians) * Math.Cos(bearingRadians));
            double lon2 = lon1 + Math.Atan2(Math.Sin(bearingRadians) * Math.Sin(radians) * Math.Cos(lat1), Math.Cos(radians) - Math.Sin(lat1) * Math.Sin(lat2));

            return new Tuple<double, double>(RadiansToDegrees(lon2), RadiansToDegrees(lat2));
        }
    }
}
