using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMSmith.GeospatialTools.Tools
{
    public static class LineOnLine
    {
        public static IEnumerable<IPoint> Execute(IMultiLine line1, IMultiLine line2)
        {
            IEnumerable<IPoint> intersections = LineIntersect.Execute(line1, line2);
            if (intersections != null && intersections.Count() > 0)
            {
                return intersections;
            }
            return null;
        }
        public static bool ExecuteBoolean(IMultiLine line1, IMultiLine line2)
        {
            IEnumerable<IPoint> intersections = LineIntersect.Execute(line1, line2);
            if (intersections != null && intersections.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
