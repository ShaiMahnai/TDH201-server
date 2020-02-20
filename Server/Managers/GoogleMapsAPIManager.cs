using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TDH.Objects;
using static Server.Managers.StreetCoordinatesManager;
using static TDH.Objects.GeoCodeResponse.Result;
using static TDH.Objects.GeoCodeResponse.Result.Geometry;

namespace TDH.Managers
{
    class GoogleMapsAPIManager
    {
        public static string GeoCode_API_KEY = "AIzaSyA_uIHH16s0jH41ozgp9ekDxht1b5Vc65U";
        public static string URL = "https://maps.googleapis.com/maps/api/geocode/json?{0}={1}&key={2}&sensor=false";
        const string City = "באר שבע";

        public GeoCodeResponse GetLcationByLatLng(string lat, string lng)
        {
            string uri = string.Format(URL, "latlng", lat + "," + lng, GeoCode_API_KEY);
            return CallGeoCode(uri);

        }

        public string GetAddressByLatLng(string lat, string lng)
        {
            GeoCodeResponse response = GetLcationByLatLng(lat, lng);
            return response.results[0].formatted_address;

        }
        public string GetStreetByLatLng(string lat, string lng)
        {
            GeoCodeResponse response = GetLcationByLatLng(lat, lng);
            List<Address_component> allAddress_components = new List<Address_component>();
            foreach (var res in response.results)
            {
                foreach (var address_components in res.address_components)
                {
                    allAddress_components.Add(address_components);
                }
            }
            Address_component ac = allAddress_components.First(address_component => Array.FindIndex(address_component.types, x => x.Equals("route")) > -1);
            return ac.long_name;
        }

        public List<LatLng> GetStreetCoordinatesList(string street)
        {
            List<LatLng> coordinates = null;
            try
            {
                GeoCodeResponse geoCodeResponse = GetStreetCoordinates(street);
                if (geoCodeResponse != null)
                {
                    //if (geoCodeResponse.results[0].geometry.bounds != null)
                    //{

                    //    coordinates = new List<LatLng>()
                    //    {geoCodeResponse.results[0].geometry.bounds.northeast,
                    //    geoCodeResponse.results[0].geometry.bounds.southwest };
                    //}
                    //else
                    //{
                        coordinates = new List<LatLng>() {
                            new LatLng() {
                                lat =geoCodeResponse.results[0].geometry.location.lat,
                                lng = geoCodeResponse.results[0].geometry.location.lng
                                              } };
                   // }
                }
            }
            catch (Exception e)
            {
            }

            return coordinates;

        }

        private GeoCodeResponse GetStreetCoordinates(string street)
        {
            string address = street + " " + City;
            string requestUri = string.Format(URL, "address", address, GeoCode_API_KEY);
            return CallGeoCode(requestUri);
        }

        private GeoCodeResponse CallGeoCode(string requestUri)
        {
            try
            {
                WebRequest request = WebRequest.Create(requestUri);
                WebResponse response = request.GetResponse();
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    String responseString = reader.ReadToEnd();
                    var res = JsonConvert.DeserializeObject<GeoCodeResponse>(responseString);
                    return res;
                }

            }
            catch (Exception e)
            {
            }
            return null;

        }


    }


}
