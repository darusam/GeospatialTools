using System;
using System.Collections.Generic;
using System.Text;

namespace CDMSmith.GeospatialTools.Geo
{
    public interface ISpatialReference
    {
        int WellKnownId
        {
            get;
        }

        int LatestWellKnownId
        {
            get;
        }
    }
}
