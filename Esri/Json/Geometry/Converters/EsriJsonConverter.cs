using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMSmith.GeospatialTools.Esri.Json.Geometry.Converters
{
    public class EsriJsonConverter : JsonConverter
    {
        public EsriJsonConverter()
        {

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<String> GetKeys(JObject jsonObject)
        {
            String[] toReturn = new String[jsonObject.Count];
            int i = 0;
            foreach(var kvp in jsonObject)
            {
                toReturn[i] = kvp.Key;
                i++;
            }
            return toReturn;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            EsriJsonObject geometry = default(EsriJsonObject);

            IEnumerable<String> keys = GetKeys(jsonObject);
            if(keys.Contains("points"))
            {
                geometry = new Multipoint();
            }
            else if(keys.Contains("rings"))
            {
                geometry = new Polygon();
            }
            else if(keys.Contains("paths"))
            {
                geometry = new Polyline();
            }
            else if(keys.Contains("x") && keys.Contains("y"))
            {
                geometry = new Point();
            }
            //else
            //{
            //    throw new NotSupportedException("Conversion of this type is not supported");
            //}

            serializer.Populate(jsonObject.CreateReader(), geometry);

            return geometry;
        }

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(EsriJsonObject);
        }
    }
}
