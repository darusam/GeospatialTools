using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CDMSmith.GeospatialTools.Geo;
using CDMSmith.GeospatialTools.Esri.Json;

namespace CDMSmith.GeospatialTools.Tools
{
    public class NearestPointOnLine
    {
        public static Result Execute(IPoint point = null, IGeometry lines = null)
        {
            if (point == null) { throw new Exception("point is a required parameter"); }
            if (lines == null) { throw new Exception("lines is a required parameter"); }

            IPoint closestPoint = new Point(Double.PositiveInfinity, Double.PositiveInfinity);
            double closestPointDistance = Double.PositiveInfinity;
            double closestPointLocation = Double.PositiveInfinity;

            int closestPointIdx = -1;


            double length = 0.0;
            foreach(IEnumerable<IPoint> line in lines.GetCoordinates())
            {
                IPoint start = null, end = null;
                int startIdx = -1, endIdx = -1;
                foreach (IPoint coord in line)
                {
                    start = end;
                    startIdx = endIdx;
                    end = coord;
                    endIdx++;
                    if (start == null)
                    {
                        continue;
                    }
                    double startDistance = point.DistanceTo(start);
                    double endDistance = point.DistanceTo(end);
                    double sectionLength = start.DistanceTo(end);

                    double intersectDistance = double.PositiveInfinity;
                    double intersectlocation = double.PositiveInfinity;

                    double bearing = start.BearingTo(end);
                    double maxDistance = Math.Max(startDistance, endDistance);

                    IPoint ppt1 = point.Destination(maxDistance, bearing + 90);
                    IPoint ppt2 = point.Destination(maxDistance, bearing - 90);

                    IEnumerable<IPoint> intersect = Intersect.Execute(
                        LineString.Create(ppt1, ppt2),
                        LineString.Create(start, end));

                    IPoint intersectPt = null;
                    if(intersect.Count() > 0)
                    {
                        intersectPt = intersect.First();
                        intersectDistance = point.DistanceTo(intersectPt);
                        intersectlocation = length + start.DistanceTo(intersectPt);
                    }

                    if(startDistance < closestPointDistance)
                    {
                        closestPoint = start;
                        closestPointIdx = startIdx;
                        closestPointLocation = length;
                        closestPointDistance = startDistance;
                    }

                    if(endDistance < closestPointDistance)
                    {
                        closestPoint = end;
                        closestPointIdx = endIdx;
                        closestPointLocation = length + sectionLength;
                        closestPointDistance = endDistance;
                    }

                    if(intersectPt != null && intersectDistance < closestPointDistance)
                    {
                        closestPoint = intersectPt;
                        closestPointIdx = startIdx;
                        closestPointDistance = intersectDistance;
                        closestPointLocation = intersectlocation;
                    }

                    length += sectionLength;
                }
            }

            return Result.Create(closestPoint, closestPointDistance, closestPointLocation);
        }

        public class Result
        {
            public IPoint Point { get; private set; }
            public double Distance { get; private set; }
            public double Location { get; private set; }

            protected internal static Result Create(IPoint point, double distance, double location)
            {
                return new Result() { Point = point, Distance = distance, Location = location };
            }
        }
    }
}
