using System.Collections.Generic;

namespace CDMSmith.GeospatialTools.Geo
{
    public interface IMultiPoint : IGeometry
    {
        bool HasM { get; set; }
        bool HasZ { get; set; }
        IEnumerable<IPoint> Points { get; }
    }
}