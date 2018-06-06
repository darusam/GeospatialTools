using System;
using System.Collections.Generic;
using CDMSmith.GeospatialTools.Esri.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EsriJson.Net.Geometry.Converters
{
    public class RingPointConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            RingPoint point = value as RingPoint;
            writer.WriteStartArray();
            writer.WriteValue(point.X);
            writer.WriteValue(point.Y);
            if (!double.IsNaN(point.Z)) writer.WriteValue(point.Z);
            if (!double.IsNaN(point.M)) writer.WriteValue(point.M);
            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            double x, y, z, m;
            if(reader.TokenType != JsonToken.Null)
            {
                if(reader.TokenType == JsonToken.StartArray)
                {
                    JToken token = JToken.Load(reader);
                    List<double> values = token.ToObject<List<double>>();
                    x = values[0];
                    y = values[1];
                    z = (values.Count > 2) ? values[2] : double.NaN;
                    m = (values.Count > 3) ? values[3] : double.NaN;

                    return new RingPoint(x, y, z, m);
                }
            }
            return null;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (RingPoint);
        }
    }
}