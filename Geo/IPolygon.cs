using System.Collections.Generic;

namespace CDMSmith.GeospatialTools.Geo
{
    public interface IPolygon : IGeometry
    {
        bool HasM { get; set; }
        bool HasZ { get; set; }
        IEnumerable<IPoint[]> Rings { get; }
    }
}