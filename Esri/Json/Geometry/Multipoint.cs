using CDMSmith.GeospatialTools.Geo;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CDMSmith.GeospatialTools.Esri.Json
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Multipoint : EsriJsonObject, IGeometry, IMultiPoint
    {
        public Multipoint()
        {

        }

        [JsonProperty(PropertyName = "hasM", Required = Required.Default)]
        public bool HasM { get; set; }

        [JsonProperty(PropertyName = "hasZ", Required = Required.Default)]
        public bool HasZ { get; set; }

        [JsonProperty(PropertyName = "points", Required = Required.Always)]
        public IList<RingPoint> Points { get; set; }

        public override string Type { get { return "multipoint"; } }

        IEnumerable<IPoint> IMultiPoint.Points => Points;

    }
}