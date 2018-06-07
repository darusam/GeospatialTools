using System;
using System.Collections.Generic;
using System.Linq;
using CDMSmith.GeospatialTools.Geo;
using CDMSmith.GeospatialTools.Esri.Json;

namespace CDMSmith.GeospatialTools.Tools
{
    public class PointInPolygon
    {
        public static bool Execute(IPoint point = null, IPolygon polygon = null)
        {
            if (point == null) { throw new Exception("point is a required parameter"); }
            if (polygon == null) { throw new Exception("polygon is a required parameter"); }
            
            BBox bbox = polygon.GetBBox();

            if(!point.Within(bbox))
            {
                return false;
            }
            
            bool found = false;
            foreach(IPoint[] ring in polygon.Rings)
            {
                if(withinRing(point, ring))
                {
                    found = !found;
                }
            }

            return found;
        }

        private static bool withinRing(IPoint point, IEnumerable<IPoint> ring)
        {
            bool inside = false;

            if(ring.First() == ring.Last())
            {
                ring = ring.Reverse().Skip(1).Reverse();
            }

            IPoint trail = ring.Last();
            foreach(IPoint coordinate in ring)
            {
                bool onBoundary = (point.Y * (coordinate.X - trail.X) + coordinate.Y * (trail.X - point.X) + trail.Y * (point.X - coordinate.X) == 0) &&
                    ((coordinate.X - point.X) * (trail.X - point.X) <= 0) &&
                    ((coordinate.Y - point.Y) * (trail.Y - point.Y) <= 0);

                if (onBoundary)
                {
                    return true;
                }

                bool intersects = ((coordinate.Y > point.Y) != (trail.Y > point.Y)) &&
                    (point.X < (trail.X - coordinate.X) * (point.Y - coordinate.Y) / (trail.Y - coordinate.Y) + coordinate.X);

                if(intersects)
                {
                    inside = !inside;
                }


                trail = coordinate;
            }

            return inside;
        }
    }
}
