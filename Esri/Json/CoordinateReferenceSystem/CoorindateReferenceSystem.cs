using CDMSmith.GeospatialTools.Geo;
using Newtonsoft.Json;

namespace CDMSmith.GeospatialTools.Esri.Json
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CRS : ISpatialReference
    {
        [JsonProperty(PropertyName = "wkid")]
        public int WellKnownId { get; set; }

        [JsonProperty(PropertyName = "latestWkid")]
        public int LatestWellKnownId { get; set; }

        [JsonProperty(PropertyName = "vcsWkid")]
        public int VCSWellKnownId { get; set; }

        [JsonProperty(PropertyName = "latestVcsWkid")]
        public int LatestVCSWellKnownId { get; set; }

        [JsonProperty(PropertyName = "wkt")]
        public string WellKnownText { get; set; }
    }
}