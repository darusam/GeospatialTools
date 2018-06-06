using CDMSmith.GeospatialTools.Geo;
using CDMSmith.GeospatialTools.Esri.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using RBush;

namespace CDMSmith.GeospatialTools.Tools
{
    public static class Extensions
    {
        public static RBush.Envelope AsEnvelope(this BBox bbox)
        {
            return new RBush.Envelope(bbox.XMin, bbox.YMin, bbox.XMax, bbox.YMax);
        }

        class SpatialDataWrapper : ISpatialData
        {
            private IEnumerable<IEnumerable<IPoint>> _points;
            private IGeometry _geometry;
            private Segment _segment;

            private Envelope _envelope;

            public ref readonly Envelope Envelope => ref _envelope;

            public SpatialDataWrapper(IEnumerable<IPoint> points)
            {
                _points = new IEnumerable<IPoint>[] { points };
                _envelope = _points.GetBBox().AsEnvelope();
            }

            public SpatialDataWrapper(IEnumerable<IEnumerable<IPoint>> points)
            {
                _points = points;
                _envelope = _points.GetBBox().AsEnvelope();
            }

            public SpatialDataWrapper(IGeometry geometry)
            {
                _geometry = geometry;
                _envelope = _geometry.GetBBox().AsEnvelope();
            }
            public SpatialDataWrapper(Segment segment)
            {
                _envelope = segment.BBox.AsEnvelope();
            }
        }

        public static RBush.ISpatialData AsSpatialData(this IEnumerable<IPoint> points)
        {
            return new SpatialDataWrapper(points);
        }

        public static RBush.ISpatialData AsSpatialData(this IEnumerable<IEnumerable<IPoint>> points)
        {
            return new SpatialDataWrapper(points);
        }

        public static RBush.ISpatialData AsSpatialData(this CDMSmith.GeospatialTools.Geo.IGeometry geometry)
        {
            return new SpatialDataWrapper(geometry);
        }

        public static RBush.ISpatialData AsSpatialData(this Segment segment)
        {
            return new SpatialDataWrapper(segment);
        }
        
        public static BBox GetBBox(this IEnumerable<IEnumerable<IPoint>> coordinates)
        {
            double maxX = double.NegativeInfinity;
            double minX = double.PositiveInfinity;
            double maxY = double.NegativeInfinity;
            double minY = double.PositiveInfinity;
            foreach (IEnumerable<IPoint> points in coordinates)
            {
                foreach (IPoint point in points)
                {
                    if (point.X > maxX) { maxX = point.X; }
                    if (point.X < minX) { minX = point.X; }
                    if (point.Y > maxX) { maxY = point.Y; }
                    if (point.Y < minY) { minY = point.X; }
                }
            }

            return new BBox() { XMin = minX, XMax = maxX, YMin = minY, YMax = maxY };
        }
        public static BBox GetBBox(this CDMSmith.GeospatialTools.Geo.IGeometry geometry)
        {
            IEnumerable<IEnumerable<IPoint>> coordinates = geometry.GetCoordinates();
            return GetBBox(coordinates);
        }
        public static BBox GetBBox(this IFeature feature)
        {
            return feature.Geometry.GetBBox();
        }

        public static IEnumerable<IEnumerable<IPoint>> GetCoordinates(this IGeometry geometry)
        {
            List<IPoint[]> points = new List<IPoint[]>();
            switch(geometry.Type.ToLower())
            {
                case "polygon":
                    Polygon polygon = (Polygon)geometry;
                    points.AddRange(polygon.Rings.Select(a => a.Select(b => b).Cast<IPoint>().ToArray()));
                    break;
                case "point":
                    Point point = (Point)geometry;
                    points.Add(new Point[] { point });
                    break;
                case "multipoint":
                    Multipoint multipoint = (Multipoint)geometry;
                    points.AddRange(multipoint.Points.Select(a => new IPoint[] { a }));
                    break;
                case "polyline":
                    Polyline polyline = (Polyline)geometry;
                    points.AddRange(polyline.Paths.Select(a => a.Select(b => b).Cast<IPoint>().ToArray()));
                    break;
                case "linestring":
                    LineString line = (LineString)geometry;
                    points.Add(line.Coordinates.ToArray());
                    break;
                default:
                    throw new System.Exception();

            }
            return points.AsEnumerable();
        }

        public static IEnumerable<IEnumerable<IPoint>> GetCoordinates(this IFeature feature)
        {
            return feature.Geometry.GetCoordinates();
        }

        public static double DistanceTo(this IPoint p1, IPoint p2)
        {
            double xDist = Math.Abs(p1.X - p2.X);
            double yDist = Math.Abs(p1.Y - p2.Y);

            return Math.Sqrt((xDist * xDist) + (yDist * yDist));
        }

        public static double BearingTo(this IPoint p1, IPoint p2)
        {
            return CDMSmith.GeospatialTools.Tools.Util.Bearing(p1.X, p1.Y, p2.X, p2.Y);
        }
        public static double BearingFrom(this IPoint p1, IPoint p2)
        {
            return CDMSmith.GeospatialTools.Tools.Util.Bearing(p2.X, p2.Y,p1.X, p1.Y);
        }

        public static IPoint Destination(this IPoint point, double distance, double bearing)
        {
            Tuple<double, double> tuple = CDMSmith.GeospatialTools.Tools.Util.Destination(point.X, point.Y, distance, bearing);
            return new Point(tuple.Item1, tuple.Item2);
        }
    }
}
