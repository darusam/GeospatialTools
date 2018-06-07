using System.Collections.Generic;

namespace CDMSmith.GeospatialTools.Geo
{
    public interface IMultiLine : IGeometry
    {
        bool HasM { get; set; }
        bool HasZ { get; set; }
        IEnumerable<IEnumerable<IPoint>> Paths { get; }
    }
}