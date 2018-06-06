using System;
using System.Collections.Generic;
using System.Text;

namespace CDMSmith.GeospatialTools.Geo
{
    public interface IFeatureSet
    {
        string GeometryType { get; set; }
        ISpatialReference SpatialReference { get; set; }
        List<IFeature> Features { get; set; }
    }
}
