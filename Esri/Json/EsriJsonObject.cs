using CDMSmith.GeospatialTools.Geo;
using Newtonsoft.Json;

namespace CDMSmith.GeospatialTools.Esri.Json
{
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(CDMSmith.GeospatialTools.Esri.Json.Geometry.Converters.EsriJsonConverter))]
    public class EsriJsonObject : CDMSmith.GeospatialTools.Geo.IGeometry
    {
        [JsonProperty(PropertyName = "spatialReference")]
        public CRS CRS { get; set; }

        [JsonProperty(PropertyName = "type")]
        public virtual string Type { get; set; }

        ISpatialReference IGeometry.CRS => CRS;
    }
}