using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDH.Objects
{
    public class GeoCodeResponse
    {
        public Plus_code plus_code { get; set; }
        public Result[] results { get; set; }
        public string status { get; set; }
        public class Plus_code
        {
            public string compound_code { get; set; }
            public string global_code { get; set; }
        }
        public class Result
        {
            public Address_component[] address_components { get; set; }
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string place_id { get; set; }
            public Plus_code plus_code { get; set; }
            public class Address_component
            {
                public string long_name { get; set; }
                public string short_name { get; set; }
                public string[] types { get; set; }
            }
            public class Geometry
            {
                public Bounds bounds { get; set; }
                public LatLng location { get; set; }
                public string location_type { get; set; }
                public Viewport viewport { get; set; }

                public class Bounds
                {
                    public LatLng northeast { get; set; }
                    public LatLng southwest { get; set; }
                }
                public class LatLng
                {
                    public double lat { get; set; }
                    public double lng { get; set; }

                }

                public class Viewport
                {
                    LatLng northeast { get; set; }
                    LatLng southwest { get; set; }

                }

            }
        }
    }
}


