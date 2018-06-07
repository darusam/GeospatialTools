using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMSmith.GeospatialTools.Tools
{
    public class Disjoint
    {
        public static bool Execute(IGeometry feature1, IGeometry feature2)
        {
            if (feature1 is IPoint)
            {
                if (feature2 is IPoint)
                {
                    return !compareCoordinates((IPoint) feature1, (IPoint) feature2);
                }
                else if(feature2 is IMultiPoint)
                {
                    return !compareCoordinates((IPoint) feature1, (IMultiPoint) feature2);
                }
                else if(feature2 is IMultiLine)
                {
                    return !pointOnLine((IPoint) feature1, feature2);
                }
                else if(feature2 is IPolygon)
                {
                    return !PointInPolygon.Execute((IPoint) feature1, (IPolygon) feature2);
                }
            }
            else if(feature1 is IMultiPoint)
            {
                if (feature2 is IPoint)
                {
                    return !compareCoordinates((IPoint)feature2, (IMultiPoint)feature1);
                }
                else if (feature2 is IMultiPoint)
                {
                    return !compareCoordinates((IMultiPoint)feature1, (IMultiPoint)feature2);
                }
                else if(feature2 is IMultiLine)
                {
                    return !pointOnLine((IMultiPoint) feature1, feature2);
                }
                else if(feature2 is IPolygon)
                {
                    return !pointsInPolygon((IMultiPoint) feature1, (IPolygon) feature2);
                }
            }
            else if(feature1 is IMultiLine)
            {
                if (feature2 is IPoint)
                {
                    return !pointOnLine((IPoint) feature2, (IMultiLine) feature1);
                }
                else if (feature2 is IMultiPoint)
                {
                    return !pointOnLine((IMultiPoint)feature2, feature1);
                }
                else if (feature2 is IMultiLine)
                {
                    return !lineOnLine((IMultiLine)feature1, (IMultiLine)feature2);
                }
                else if (feature2 is IPolygon)
                {
                    return !lineInPolygon((IMultiLine)feature1, (IPolygon)feature2);
                }
            }
            else if(feature1 is IPolygon)
            {
                if (feature2 is IPoint)
                {
                    return !PointInPolygon.Execute((IPoint)feature2, (IPolygon)feature1);
                }
                else if (feature2 is IMultiPoint)
                {
                    return !pointsInPolygon((IMultiPoint)feature2, (IPolygon)feature1);
                }
                else if (feature2 is IMultiLine)
                {
                    return !lineInPolygon((IMultiLine)feature2, (IPolygon)feature1);
                }
                else if (feature2 is IPolygon)
                {
                    return !polygonInPolygon((IPolygon)feature1, (IPolygon)feature2);
                }
            }

            return false;
        }

        private static bool compareCoordinates(IPoint p1, IPoint p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        private static bool compareCoordinates(IPoint p1, IMultiPoint p2)
        {
            foreach(IPoint point in p2.Points)
            {
                if(compareCoordinates(p1, point))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool compareCoordinates(IMultiPoint p1, IMultiPoint p2)
        {
            foreach(IPoint point in p1.Points)
            {
                if(compareCoordinates(point, p2))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool pointsInPolygon(IMultiPoint points, IPolygon poly)
        {
            foreach(IPoint point in points.Points)
            {
                if(PointInPolygon.Execute(point, poly))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool pointOnLine(IMultiPoint points, IGeometry line)
        {
            foreach(IPoint point in points.Points)
            {
                if(pointOnLine(point, line))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool pointOnLine(IPoint point, IGeometry line)
        {
            IPoint previous = null;
            foreach(IPoint current in line.GetCoordinates().First())
            {
                if(previous != null)
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
            if(cross != 0)
            {
                return false;
            }

            if(Math.Abs(dxl) >= Math.Abs(dyl))
            {
                if(dxl > 0)
                {
                    return start.X <= point.X && point.X <= end.X;
                }
                else
                {
                    return end.X <= point.X && point.X <= start.X;
                }
            }
            else if(dyl > 0)
            {
                return start.Y <= point.Y && point.Y <= end.Y;
            }
            else
            {
                return end.Y <= point.Y && point.Y <= start.Y;
            }
        }



        private static bool lineOnLine(IMultiLine line1, IMultiLine line2)
        {
            IEnumerable<IPoint> intersections = LineIntersect.Execute(line1, line2);
            if(intersections != null && intersections.Count() > 0)
            {
                return true;
            }
            return false;
        }

        private static bool lineInPolygon(IMultiLine lines, IPolygon poly)
        {
            foreach(IEnumerable<IPoint> line in lines.GetCoordinates())
            {
                if(lineInPolygon(line, poly))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool lineInPolygon(IEnumerable<IPoint> line, IPolygon poly)
        {
            foreach(IPoint point in line)
            {
                if(PointInPolygon.Execute(point, poly))
                {
                    return true;
                }
            }
            IEnumerable<IPoint> intersections = LineIntersect.Execute(LineString.Create(line.ToArray()), PolygonToLine.Execute(poly));
            if(intersections != null && intersections.Count() > 0)
            {
                return true;
            }
            return false;
        }

        private static bool polygonInPolygon(IPolygon poly1, IPolygon poly2)
        {
            foreach(IPoint point in poly1.GetCoordinates().First())
            {
                if(PointInPolygon.Execute(point, poly2))
                {
                    return true;
                }
            }
            foreach(IPoint point in poly2.GetCoordinates().First())
            {
                if(PointInPolygon.Execute(point, poly1))
                {
                    return true;
                }
            }

            IEnumerable<IPoint> intersections = LineIntersect.Execute(PolygonToLine.Execute(poly1), PolygonToLine.Execute(poly2));

            if(intersections != null && intersections.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
