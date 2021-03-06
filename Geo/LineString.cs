﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CDMSmith.GeospatialTools.Geo
{
    public class LineString : IGeometry
    {
        public IPoint[] Coordinates { get; set; }
        public ISpatialReference CRS { get; set; }

        //public IEnumerable<IPoint[]> Paths { get; set; }

        public string Type => "lineString";

        public bool HasM { get; set; }
        public bool HasZ { get; set; }
        

        public static LineString Create(params IPoint[] verticies)
        {
            LineString lineString = new LineString();
            lineString.Coordinates = verticies;

            return lineString;
        }
    }

}
