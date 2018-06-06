using System;
using System.Collections.Generic;
using System.Text;

namespace CDMSmith.GeospatialTools.Geo
{
    public interface IFeature
    {
        IGeometry Geometry { get; }
        IDictionary<string, object> Attributes { get; }
    }
}
