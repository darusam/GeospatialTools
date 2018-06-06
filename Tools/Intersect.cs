using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using RBush;
using CDMSmith.GeospatialTools.Esri.Json;

namespace CDMSmith.GeospatialTools.Tools
{
    public class Intersect
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

            double denom = ((points2.Last().Y - points2.First().Y) * (points1.Last().X - points1.First().X)) - ((points2.Last().X - points2.First().X) * (points1.Last().Y - points1.First().Y));
            double numeA = ((points2.Last().X - points2.First().X) * (points1.First().Y - points2.First().Y)) - ((points2.Last().Y - points2.First().Y) * (points1.First().X - points2.First().X));
            double numeB = ((points1.Last().X - points1.First().X) * (points1.First().Y - points2.First().Y)) - ((points1.Last().Y - points1.First().Y) * (points1.First().X - points2.First().X));

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
                    (points1.First().X + (uA * (points1.Last().X - points1.First().X))), 
                    (points1.First().Y + (uA * (points1.Last().Y - points1.First().Y))));
            }
            return null;
        }
    }
}
