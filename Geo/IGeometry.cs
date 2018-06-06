using System;
using System.Collections.Generic;
using System.Text;

namespace CDMSmith.GeospatialTools.Geo
{
    public interface IGeometry
    {
        ISpatialReference CRS { get; }
        string Type { get; }
    }
}
