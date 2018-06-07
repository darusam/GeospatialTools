using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMSmith.GeospatialTools.Tools
{
    public static class CompareCoordinates
    {
        public static IEnumerable<IPoint> Execute(IPoint p1, IPoint p2)
        {
            if(p1.X == p2.X && p1.Y == p2.Y)
            {
                yield return p1;
            }
        }

        public static IEnumerable<IPoint> Execute(IPoint p1, IMultiPoint p2)
        {
            foreach (IPoint point in p2.Points)
            {
                IEnumerable<IPoint> points = Execute(p1, point);
                if (points != null && points.Count() > 0)
                {
                    foreach(IPoint p in points)
                    {
                        yield return p;
                    }
                }
            }
        }
        public static IEnumerable<IPoint> Execute(IMultiPoint p1, IMultiPoint p2)
        {
            foreach (IPoint point in p1.Points)
            {
                IEnumerable<IPoint> points = Execute(point, p2);
                if (points != null && points.Count() > 0)
                {
                    foreach (IPoint p in points)
                    {
                        yield return p;
                    }
                }
            }
        }
    }
}
