using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using RBush;
using CDMSmith.GeospatialTools.Esri.Json;

namespace CDMSmith.GeospatialTools.Tools
{
    public class LineIntersect
    {
        public static IEnumerable<IPoint> Execute(IGeometry line1, IGeometry line2)
        {
            IDictionary<string, IPoint> unique = new Dictionary<string,IPoint>();
            IList<IPoint> intersections = new List<IPoint>();

            RBush<ISpatialData> tree = new RBush<ISpatialData>();
            tree.BulkLoad(LineSegment.Execute(line2).Select(a => a.AsSpatialData()));
            foreach(Segment segment in LineSegment.Execute(line1))
            {
                foreach(ISpatialData match in tree.Search(segment.BBox.AsEnvelope()))
                {
                    IPoint intersect = Intersects(segment.LineString, (IGeometry)match);
                    if(intersect != null)
                    {
                        var key = string.Join(",", intersect.X, intersect.Y);
                        if(!unique.ContainsKey(key))
                        {
                            unique.Add(key, intersect);
                            intersections.Add(intersect);
                        }
                    }
                }
            }

            return intersections;
        }

        private static IPoint Intersects(IGeometry line1, IGeometry line2)
        {
            IEnumerable<IPoint> points1 = line1.GetCoordinates().First();
            IEnumerable<IPoint> points2 = line2.GetCoordinates().First();

            if(points1.Count() != 2)
            {
                throw new Exception("Intersects: line1 must contain only 2 points");
            }
            if(points2.Count() != 2)
            {
                throw new Exception("Intersects: line2 must contain only 2 points");
            }

            IPoint p1f = points1.First();
            IPoint p1l = points1.Last();
            IPoint p2f = points2.First();
            IPoint p2l = points2.Last();

            double denom = ((p2l.Y - p2f.Y) * (p1l.X - p1f.X)) - ((p2l.X - p2f.X) * (p1l.Y - p1f.Y));
            double numeA = ((p2l.X - p2f.X) * (p1f.Y - p2f.Y)) - ((p2l.Y - p2f.Y) * (p1f.X - p2f.X));
            double numeB = ((p1l.X - p1f.X) * (p1f.Y - p2f.Y)) - ((p1l.Y - p1f.Y) * (p1f.X - p2f.X));

            if(denom == 0)
            {
                if(numeA == numeB && numeB == 0)
                {
                    return null;
                }
                return null;
            }

            double uA = numeA / denom;
            double uB = numeB / denom;

            if(uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                return new Point(
                    (p1f.X + (uA * (p1l.X - p1f.X))), 
                    (p1f.Y + (uA * (p1l.Y - p1f.Y))));
            }
            return null;
        }
    }
}
