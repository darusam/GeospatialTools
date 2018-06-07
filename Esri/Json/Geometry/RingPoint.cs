using CDMSmith.GeospatialTools.Geo;
using EsriJson.Net.Geometry.Converters;
using Newtonsoft.Json;

namespace CDMSmith.GeospatialTools.Esri.Json
{
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(RingPointConverter))]
    public class RingPoint : IPoint
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public double M { get; set; }

        public ISpatialReference CRS => throw new System.NotImplementedException();

        public string Type => throw new System.NotImplementedException();

        public RingPoint(double x, double y, double z = double.NaN, double m = double.NaN)
        {
            X = x;
            Y = y;
            if (double.IsNaN(z) == false) { Z = z; }
            if (double.IsNaN(m) == false) { M = m; }
        }

        public bool Equals(RingPoint obj)
        {
            return obj != null && obj.X == X && obj.Y == Y && obj.Z == Z && obj.M == M;
        }
    }
}