using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDMSmith.GeospatialTools.Geo
{
    public class MultiLineString : IGeometry, IMultiLine
    {
        public IPoint[][] Coordinates { get; set; }
        public ISpatialReference CRS { get; set; }

        //public IEnumerable<IPoint[]> Paths { get; set; }

        public string Type => "multiLineString";

        public bool HasM { get; set; }
        public bool HasZ { get; set; }

        public IEnumerable<IEnumerable<IPoint>> Paths => Coordinates;

        public static MultiLineString Create(params IPoint[][] verticies)
        {
            MultiLineString lineString = new MultiLineString();
            lineString.Coordinates = verticies;

            return lineString;
        }
    }
}
