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
        public static IEnumerable<IPoint> Execute(IMultiLine lines, IPolygon poly)
        {
            foreach(IEnumerable<IPoint> line in lines.GetCoordinates())
            {
                IEnumerable<IPoint> sharedPoints = Execute(line, poly);
                if (sharedPoints != null)
                {
                    foreach (IPoint point in sharedPoints)
                    {
                        yield return point;
                    }
                }
            }
        }

        public static IEnumerable<IPoint> Execute(IEnumerable<IPoint> line, IPolygon poly)
        {
            IPoint previous = null;
            IGeometry polygonLine = PolygonToLine.Execute(poly);
            foreach (IPoint point in line)
            {
                if (PointInPolygon.Execute(point, poly))
                {
                    yield return point;
                }
                if (previous != null)
                {
                    IEnumerable<IPoint> intersections = LineIntersect.Execute(LineString.Create(previous, point), polygonLine);
                    if (intersections != null)
                    {
                        foreach (IPoint intersection in intersections)
                        {
                            yield return intersection;
                        }
                    }
                }

                previous = point;
            }
        }

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
