using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMSmith.GeospatialTools.Tools
{
    public static class LineInPolygon
    {
        public static bool ExecuteBoolean(IMultiLine lines, IPolygon poly)
        {
            foreach (IEnumerable<IPoint> line in lines.GetCoordinates())
            {
                if (ExecuteBoolean(line, poly))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ExecuteBoolean(IEnumerable<IPoint> line, IPolygon poly)
        {
            foreach (IPoint point in line)
            {
                if (PointInPolygon.Execute(point, poly))
                {
                    return true;
                }
            }
            IEnumerable<IPoint> intersections = LineIntersect.Execute(LineString.Create(line.ToArray()), PolygonToLine.Execute(poly));
            if (intersections != null && intersections.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
