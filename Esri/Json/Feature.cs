using System.Collections.Generic;
using CDMSmith.GeospatialTools.Geo;
using EsriJson.Net.Geometry;
using Newtonsoft.Json;

namespace CDMSmith.GeospatialTools.Esri.Json
{
    public class Feature : CDMSmith.GeospatialTools.Geo.IFeature
    {
        public Feature(EsriJsonObject geometry, Dictionary<string, object> attributes)
        {
            Attributes = attributes;
            Geometry = geometry;
        }

        [JsonProperty(PropertyName = "attributes", Required = Required.AllowNull)]
        public Dictionary<string, object> Attributes { get; private set; }

        [JsonProperty(PropertyName = "geometry", Required = Required.AllowNull)]
        public EsriJsonObject Geometry { get; set; }

        Geo.IGeometry IFeature.Geometry => Geometry;

        IDictionary<string, object> IFeature.Attributes => Attributes;
    }
}