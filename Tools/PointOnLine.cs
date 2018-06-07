using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMSmith.GeospatialTools.Tools
{
    public static class PointOnLine
    {
        public static IEnumerable<IPoint> Execute(IMultiPoint points, IGeometry line)
        {
            foreach (IPoint point in points.Points)
            {
                if (ExecuteBoolean(point, line))
                {
                    yield return point;
                }
            }
        }
        public static IEnumerable<IPoint> Execute(IPoint point, IGeometry line)
        {
            if (ExecuteBoolean(point, line))
            {
                yield return point;
            }
        }

        public static bool ExecuteBoolean(IMultiPoint points, IGeometry line)
        {
            foreach (IPoint point in points.Points)
            {
                if (ExecuteBoolean(point, line))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ExecuteBoolean(IPoint point, IGeometry line)
        {
            IPoint previous = null;
            foreach (IPoint current in line.GetCoordinates().First())
            {
                if (previous != null)
                {
                    if (pointOnLineSegment(point, previous, current))
                    {
                        return true;
                    }
                }
                previous = current;
            }

            return false;
        }

        private static bool pointOnLineSegment(IPoint point, IPoint start, IPoint end)
        {
            double dxc = point.X - start.X;
            double dyc = point.Y - start.Y;

            double dxl = end.X - start.X;
            double dyl = end.Y - start.Y;

            double cross = dxc * dyl - dyc * dxl;
            if (cross != 0)
            {
                return false;
            }

            if (Math.Abs(dxl) >= Math.Abs(dyl))
            {
                if (dxl > 0)
                {
                    return start.X <= point.X && point.X <= end.X;
                }
                else
                {
                    return end.X <= point.X && point.X <= start.X;
                }
            }
            else if (dyl > 0)
            {
                return start.Y <= point.Y && point.Y <= end.Y;
            }
            else
            {
                return end.Y <= point.Y && point.Y <= start.Y;
            }
        }
    }
}
