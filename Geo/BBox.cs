using System;
using System.Collections.Generic;
using System.Text;

namespace CDMSmith.GeospatialTools.Geo
{
    public class BBox
    {
        public double XMin { get; set; }
        public double XMax { get; set; }
        public double YMin { get; set; }
        public double YMax { get; set; }
    }

    public static class BBoxExtensions
    {
        public static bool Within(this IPoint point, BBox bbox)
        {
            if (point.X >= bbox.XMin && point.X <= bbox.XMax && point.Y >= bbox.YMin && point.Y <= bbox.YMax)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Contains(this BBox bbox, IPoint point)
        {
            if (point.X >= bbox.XMin && point.X <= bbox.XMax && point.Y >= bbox.YMin && point.Y <= bbox.YMax)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
