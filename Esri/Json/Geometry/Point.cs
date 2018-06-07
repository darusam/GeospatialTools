using CDMSmith.GeospatialTools.Geo;
using Newtonsoft.Json;

namespace CDMSmith.GeospatialTools.Esri.Json
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Point : EsriJsonObject, IGeometry, IPoint
    {
        public Point()
        {
            
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        [JsonProperty(PropertyName = "x", Required = Required.Always)]
        public double X { get; set; }

        [JsonProperty(PropertyName = "y", Required = Required.Always)]
        public double Y { get; set; }

        [JsonProperty(PropertyName = "z", Required = Required.Default)]
        public double Z { get; set; }

        [JsonProperty(PropertyName = "m", Required = Required.Default)]
        public double M { get; set; }

        public bool Equals(Point obj)
        {
            return obj != null && obj.X == X && obj.Y == Y;
        }

        public override string Type { get { return "point"; } }
    }
}