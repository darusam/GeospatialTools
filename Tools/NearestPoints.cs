using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CDMSmith.GeospatialTools.Tools
{
    public class NearestPoints
    {
        public static Result Execute(IFeatureSet featureSetA, IFeatureSet featureSetB)
        {
            double distance = double.PositiveInfinity;
            IPoint closestPointA = null;
            IPoint closestPointB = null;
            IFeature closestFeatureA = null;
            IFeature closestFeatureB = null;
            foreach(IFeature aFeature in featureSetA.Features)
            {
                foreach (IPoint aPoint in aFeature.Geometry.GetCoordinates().SelectMany(a => a.Select(b => b)))
                {
                    NearestPointOnLine.Result result;
                    foreach (IFeature feature in featureSetB.Features)
                    {
                        result = NearestPointOnLine.Execute(aPoint, feature.Geometry);
                        if (result.Distance < distance)
                        {
                            distance = result.Distance;
                            closestPointA = aPoint;
                            closestPointB = result.Point;

                            closestFeatureA = aFeature;
                            closestFeatureB = feature;
                        }
                    }
                }
            }

            foreach (IFeature bFeature in featureSetB.Features)
            {
                foreach (IPoint bPoint in bFeature.Geometry.GetCoordinates().SelectMany(a => a.Select(b => b)))
                {
                    NearestPointOnLine.Result result;
                    foreach (IFeature feature in featureSetA.Features)
                    {
                        result = NearestPointOnLine.Execute(bPoint, feature.Geometry);
                        if (result.Distance < distance)
                        {
                            distance = result.Distance;
                            closestPointB = bPoint;
                            closestPointA = result.Point;

                            closestFeatureA = feature;
                            closestFeatureB = bFeature;
                        }
                    }
                }
            }

            return Result.Create(
                closestPointA, 
                closestPointB, 
                distance,
                featureA: closestFeatureA, featureB: closestFeatureB,
                geometryA: closestFeatureA != null ? closestFeatureA.Geometry : null,
                geometryB: closestFeatureB != null ? closestFeatureB.Geometry : null);
        }

        public static Result Execute(IFeature featureA, IFeature featureB)
        {
            return Execute(featureA.Geometry, featureB.Geometry);
        }

        public static Result Execute(IGeometry geometryA, IGeometry geometryB)
        {
            double distance = double.PositiveInfinity;
            IPoint closestPointA = null;
            IPoint closestPointB = null;
            foreach (IPoint aPoint in geometryA.GetCoordinates().SelectMany(a => a.Select(b => b)))
            {
                NearestPointOnLine.Result result;
                result = NearestPointOnLine.Execute(aPoint, geometryA);
                if (result.Distance < distance)
                {
                    distance = result.Distance;
                    closestPointA = aPoint;
                    closestPointB = result.Point;
                }
            }
            foreach (IPoint bPoint in geometryB.GetCoordinates().SelectMany(a => a.Select(b => b)))
            {
                NearestPointOnLine.Result result;
                result = NearestPointOnLine.Execute(bPoint, geometryB);
                if (result.Distance < distance)
                {
                    distance = result.Distance;
                    closestPointB = bPoint;
                    closestPointA = result.Point;
                }
            }

            return Result.Create(closestPointA, closestPointB, distance);
        }

        public class Result
        {
            public IPoint ClosestPointA { get; private set; }
            public IPoint ClosestPointB { get; private set; }
            public double Distance { get; private set; }
            public IFeature FeatureA { get; private set; }
            public IFeature FeatureB { get; private set; }
            public IGeometry GeometryA { get; private set; }
            public IGeometry GeometryB { get; private set; }

            protected internal static Result Create(
                IPoint pointA, 
                IPoint pointB, 
                Double distance, 
                IFeature featureA = null, 
                IFeature featureB = null, 
                IGeometry geometryA = null, 
                IGeometry geometryB = null)
            {
                Result toReturn = new Result()
                {
                    ClosestPointA = pointA,
                    ClosestPointB = pointB,
                    Distance = distance,
                    FeatureA = featureA,
                    FeatureB = featureB,
                    GeometryA = geometryA,
                    GeometryB = geometryB
                };

                return toReturn;
            }
        }
    }
}
