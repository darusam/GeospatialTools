using CDMSmith.GeospatialTools.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMSmith.GeospatialTools.Tools
{
    public class PolygonToLine
    {
        public static IGeometry Execute(IGeometry polygon)
        {
            switch(polygon.Type)
            {
                case "polygon": return SinglePolygonToLine(polygon);
                case "multipolygon": throw new Exception("multiPolygon not supported");
                default: throw new Exception();
            }
        }

        private static IGeometry SinglePolygonToLine(IGeometry polygon)
        {
            IEnumerable<IEnumerable<IPoint>> coordinates = polygon.GetCoordinates();

            if(coordinates.Count() > 1)
            {
                return MultiLineString.Create(coordinates.Select(a=>a.ToArray()).ToArray());
            }
            else
            {
                return LineString.Create(coordinates.First().ToArray());
            }
        }
    }
}
