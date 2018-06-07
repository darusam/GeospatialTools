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
        public static bool Execute(IFeatureSet fs1, IFeatureSet fs2)
        {
            foreach(IFeature feature1 in fs1.Features)
            {
                foreach(IFeature feature2 in fs2.Features)
                {
                    if(Execute(feature1.Geometry, feature2.Geometry) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool Execute(IGeometry feature1, IGeometry feature2)
        {
            if (feature1 is IPoint)
            {
                if (feature2 is IPoint)
                {
                    return !BoolCompareCoordinates.Execute((IPoint) feature1, (IPoint) feature2);
                }
                else if(feature2 is IMultiPoint)
                {
                    return !BoolCompareCoordinates.Execute((IPoint) feature1, (IMultiPoint) feature2);
                }
                else if(feature2 is IMultiLine)
                {
                    return !PointOnLine.ExecuteBoolean((IPoint) feature1, feature2);
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
                    return !BoolCompareCoordinates.Execute((IPoint)feature2, (IMultiPoint)feature1);
                }
                else if (feature2 is IMultiPoint)
                {
                    return !BoolCompareCoordinates.Execute((IMultiPoint)feature1, (IMultiPoint)feature2);
                }
                else if(feature2 is IMultiLine)
                {
                    return !PointOnLine.ExecuteBoolean((IMultiPoint) feature1, feature2);
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
                    return !PointOnLine.ExecuteBoolean((IPoint) feature2, (IMultiLine) feature1);
                }
                else if (feature2 is IMultiPoint)
                {
                    return !PointOnLine.ExecuteBoolean((IMultiPoint)feature2, feature1);
                }
                else if (feature2 is IMultiLine)
                {
                    return !LineOnLine.ExecuteBoolean((IMultiLine)feature1, (IMultiLine)feature2);
                }
                else if (feature2 is IPolygon)
                {
                    return !LineInPolygon.ExecuteBoolean((IMultiLine)feature1, (IPolygon)feature2);
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
                    return !LineInPolygon.ExecuteBoolean((IMultiLine)feature2, (IPolygon)feature1);
                }
                else if (feature2 is IPolygon)
                {
                    return !PolygonInPolygon.ExecuteBoolean((IPolygon)feature1, (IPolygon)feature2);
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

        
    }
}
