using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMSmith.GeospatialTools.Tools
{
    public static class PolygonInPolygon
    {
        public static bool ExecuteBoolean(IPolygon poly1, IPolygon poly2)
        {
            foreach (IPoint point in poly1.GetCoordinates().First())
            {
                if (PointInPolygon.Execute(point, poly2))
                {
                    return true;
                }
            }
            foreach (IPoint point in poly2.GetCoordinates().First())
            {
                if (PointInPolygon.Execute(point, poly1))
                {
                    return true;
                }
            }

            IEnumerable<IPoint> intersections = LineIntersect.Execute(PolygonToLine.Execute(poly1), PolygonToLine.Execute(poly2));

            if (intersections != null && intersections.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
