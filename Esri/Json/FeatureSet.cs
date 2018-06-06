using CDMSmith.GeospatialTools.Geo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDMSmith.GeospatialTools.Esri.Json
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FeatureSet : IFeatureSet
    {
        [JsonProperty(PropertyName = "displayFieldName", Required = Required.AllowNull)]
        public string DisplayFieldName { get; set; }

        [JsonProperty(PropertyName = "fieldAliases", Required = Required.AllowNull)]
        public IEnumerable<Tuple<string, string>> FieldAliases { get; set; } = new List<Tuple<string, string>>();

        [JsonProperty(PropertyName = "geometryType", Required = Required.AllowNull)]
        public string GeometryType { get; set; }

        [JsonProperty(PropertyName = "hasZ", Required = Required.AllowNull)]
        public bool HasZ { get; set; }

        [JsonProperty(PropertyName = "hasM", Required = Required.AllowNull)]
        public bool HasM { get; set; }

        [JsonProperty(PropertyName = "spatialReference", Required = Required.AllowNull)]
        public ISpatialReference SpatialReference { get; set; }

        [JsonProperty(PropertyName = "fields", Required = Required.AllowNull)]
        public IList<FieldDefinition> Fields { get; set; } = new List<FieldDefinition>();

        [JsonProperty(PropertyName = "features", Required = Required.AllowNull)]
        public List<IFeature> Features { get; set; } = new List<IFeature>();
    }
}
