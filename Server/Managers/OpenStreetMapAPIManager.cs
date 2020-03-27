using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Server.Objects;
using static Server.Managers.StreetCoordinatesManager;
using static Server.Objects.GeoCodeResponse.Result.Geometry;

namespace Server.Managers
{

    public class OpenStreetMapAPIManager
    {
        HttpRequestsManager oHttpRequestsManager;
        const string City = "באר שבע";
        string URL = "https://nominatim.openstreetmap.org/search.php?q={0}&polygon_geojson=1&format=json";

        public OpenStreetMapAPIManager()
        {
            oHttpRequestsManager = new HttpRequestsManager();
        }
        public OpenStreetMapResponseLine Get(string street)
        {
            string requestUri = URL.Replace("{0}", street + " " + City);
            string response = string.Empty;
            bool success = false;
            while (!success)
            {
                try
                {
                    //Uri uri = new Uri(address, UriKind.Absolute);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                    request.UserAgent = "DHC 1 contact mahnai@post.bgu.ac.il";
                    response = oHttpRequestsManager.Get(request);
                    success = true;
                }
                catch (Exception e)
                {

                }
            }

            dynamic geo;
            JArray lst = JArray.Parse(response);
            if (lst.Count > 0)
            {
                bool enter = false;
                List<OpenStreetMapResponseLine> res = new List<OpenStreetMapResponseLine>();
                foreach (var item in lst)
                {
                    if (((string)item["geojson"]["type"]).Equals("LineString", StringComparison.OrdinalIgnoreCase))
                    {
                        enter = true;
                        try
                        {
                            res.Add(item.ToObject<OpenStreetMapResponseLine>());

                        }
                        catch (Exception e)
                        {
                        }
                    }
                    else
                    {

                    }
                }
                if (!enter)
                { //if no polygons
                    foreach (var item in lst)
                    {
                        if (((string)item["geojson"]["type"]).Equals("point", StringComparison.OrdinalIgnoreCase))
                        {
                            enter = true;
                            try
                            {
                                var openStreetMapResponse = item.ToObject<OpenStreetMapResponse>();
                                double[] arr = item["geojson"]["coordinates"].ToObject<double[]>();
                                var toAdd = new OpenStreetMapResponseLine(openStreetMapResponse)
                                {
                                    geojson = new OpenStreetMapResponseLine.Geojson()
                                    { type = "LineString", Coordinates = new List<double[]>() { arr } }
                                };
                                res.Add(toAdd);

                            }
                            catch (Exception e)
                            {

                                throw;
                            }
                        }
                        else
                        {
                            geo = (dynamic)item["geojson"]["coordinates"];
                        }
                    }


                }
                if (!enter)
                {
                    //File.AppendAllText(@"C:\Users\shaik\OneDrive\מסמכים\studying\university\semester 9\dhc\missingMultiPlygon.txt", street + Environment.NewLine);
                    return null;
                }
                else
                {
                    res = res.OrderBy(x => -x.geojson.Coordinates.Count).ToList();
                    return res.First();
                }

            }
            else
            {
                return null;
            }

        }

        public List<LatLng> GetStreetCoordinates(string streetName)
        {
            OpenStreetMapResponseLine res = Get(streetName);
            List<LatLng> coordinatesList = new List<LatLng>();
            if (res != null)
            {
                foreach (var coordinate in res.geojson.Coordinates)
                {
                    coordinatesList.Add(new LatLng() { lng = coordinate[0], lat = coordinate[1] });
                }
            }
            return coordinatesList;
        }

        public List<LatLng> GetStreetCoordinatesList(string street)
        {
            List<LatLng> coordinates = null;
            bool succes = false;
            string[] nameArr = street.Split(' ');
            while (!succes && nameArr.Length > 0)
            {

                string name = string.Join(" ", nameArr);
                coordinates = GetStreetCoordinates(name);
                if (coordinates.Count > 0)
                {
                    succes = true;
                }
                else
                {
                    //try to call the same request over and over with split the street's name 
                    //so "דוד בן גוריון"  not count but "בן גוריון" do
                    List<string> badArr = (new string[] { //thus names are not good to be splited, because they bring other sreets's names results if cplit
                            "שמעון בן שטח", "אסתר המלכה", "פלורי אשר", "בת שבע בן אליהו",
                            "אביתר הכהן","אורות ישראל","פולה בן גוריון" }).ToList();
                    if (street.Contains("כהן"))
                    {
                        badArr.Add(street);
                    }
                    nameArr = nameArr.Skip(1).ToArray();
                 
                    if (nameArr.Length > 0 && badArr.Contains(street))
                    {
                        nameArr = new string[] { };
                    }
                }

            }
            return coordinates;

        }




        public class OpenStreetMapResponse
        {
            public string place_id { get; set; }
            public string licence { get; set; }
            public string osm_type { get; set; }
            public string osm_id { get; set; }
            public string[] boundingbox { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public string display_name { get; set; }
            public string type { get; set; }
            public double importance { get; set; }


        }
        public class OpenStreetMapResponseLine : OpenStreetMapResponse
        {
            public OpenStreetMapResponseLine()
            {

            }
            public OpenStreetMapResponseLine(OpenStreetMapResponse openStreetMapResponse)
            {
                this.licence = openStreetMapResponse.licence;
                this.lat = openStreetMapResponse.lat;
                this.importance = openStreetMapResponse.importance;
                this.lng = openStreetMapResponse.lng;
                this.osm_id = openStreetMapResponse.osm_id;
                this.osm_type = openStreetMapResponse.osm_type;
                this.place_id = openStreetMapResponse.place_id;
                this.type = openStreetMapResponse.type;


            }

            public Geojson geojson { get; set; }

            public class Geojson
            {
                public string type { get; set; }
                public List<double[]> Coordinates { get; set; }
            }
        }


    }
}
