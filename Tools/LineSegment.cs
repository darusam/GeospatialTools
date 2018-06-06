using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CDMSmith.GeospatialTools.Tools
{
    public class LineSegment
    {
        public static IEnumerable<Segment> Execute(CDMSmith.GeospatialTools.Geo.IGeometry feature)
        {
            IList<Segment> segments = new List<Segment>();
            SegmentFeature(feature, segments);
            return segments;
        }

        private static void SegmentFeature(CDMSmith.GeospatialTools.Geo.IGeometry feature, IList<Segment> results)
        {
            IEnumerable<Segment> segments = CreateSegments(feature.GetCoordinates());
            foreach(Segment segment in segments)
            {
                segment.Id = results.Count;
                results.Add(segment);
            }
        }

        private static Segment[] CreateSegments(IEnumerable<IEnumerable<IPoint>> coordinates)
        {
            IList<Segment> toReturn = new List<Segment>();
            IPoint current = null, previous = null;
            foreach (IEnumerable<IPoint> points in coordinates)
            {
                foreach (IPoint point in points)
                {
                    previous = current;
                    current = point;
                    if (previous == null)
                    {
                        continue;
                    }
                    Segment toAdd = new Segment()
                    {
                        LineString = new LineString() { Coordinates = new IPoint[] { previous, current } }
                    };
                    toAdd.BBox = toAdd.LineString.GetBBox();

                    toReturn.Add(toAdd);
                }
            }

            return toReturn.ToArray();
        }

    }

    public class Segment : IFeature
    {
        public LineString LineString { get; set; }
        public BBox BBox { get; set; }
        public Int32 Id { get; set; }

        public IGeometry Geometry {
            get => LineString;
        }
        public IDictionary<string, object> Attributes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
