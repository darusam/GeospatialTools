using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMSmith.GeospatialTools.Tools
{
    public static class BoolCompareCoordinates
    {
        public static bool Execute(IPoint p1, IPoint p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public static bool Execute(IPoint p1, IMultiPoint p2)
        {
            foreach (IPoint point in p2.Points)
            {
                if (Execute(p1, point))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool Execute(IMultiPoint p1, IMultiPoint p2)
        {
            foreach (IPoint point in p1.Points)
            {
                if (Execute(point, p2))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
