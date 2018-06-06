using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CDMSmith.GeospatialTools.Esri.Json
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class FieldDefinition
    {
        [JsonProperty(PropertyName = "name", Required = Required.AllowNull)]
        public String Name { get; set; }
        [JsonProperty(PropertyName = "type", Required = Required.AllowNull)]
        public String Type { get; set; }
        [JsonProperty(PropertyName = "alias", Required = Required.AllowNull)]
        public String Alias { get; set; }
    }
}
