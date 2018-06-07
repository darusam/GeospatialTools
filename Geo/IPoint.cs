namespace CDMSmith.GeospatialTools.Geo
{
    public interface IPoint : IGeometry
    {
        double X { get; set; }
        double Y { get; set; }
    }
}