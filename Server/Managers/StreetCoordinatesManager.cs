using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDH.Managers;
using TDH.Objects;
using static TDH.Managers.OpenStreetMapAPIManager;
using static TDH.Objects.GeoCodeResponse.Result.Geometry;

namespace Server.Managers
{
    public class StreetCoordinatesManager
    {
        OpenStreetMapAPIManager oOpenStreetMapAPIManager;
        GoogleMapsAPIManager oGoogleMapsAPIManager;

        public StreetCoordinatesManager()
        {
            oOpenStreetMapAPIManager = new OpenStreetMapAPIManager();
            oGoogleMapsAPIManager = new GoogleMapsAPIManager();

        }

        public GetStreetCoordinatesListResponse GetStreetCoordinatesList(List<StreetsApiIResponse> StreetsApiIResponseList)
        {
            List<StreetCoordinates> streetCoordinatesList = new List<StreetCoordinates>();
            List<string> missing = new List<string>();
            bool succes = false;
            foreach (var item in StreetsApiIResponseList)
            {
                succes = false;
                List<LatLng> coordinates = oOpenStreetMapAPIManager.GetStreetCoordinatesList(item.Name);
                if (coordinates.Count > 0)
                {
                    succes = true;
                }
                else
                {
                    coordinates = oGoogleMapsAPIManager.GetStreetCoordinatesList(item.Name);
                    if (coordinates.Count > 0)
                    {
                        succes = true;
                    }
                }
                if (succes)
                {
                    int decadeOfApprovementMeeting = 
                        Helpers.DateTimeHelper.GetDecade(item.DateOfApprovementMeeting);
                    StreetCoordinates resStreetCoordinates = new StreetCoordinates(
                        item.Name, item.neighborhood, decadeOfApprovementMeeting,
                        item.IsNamedAfterWoman, coordinates);
                    streetCoordinatesList.Add(resStreetCoordinates);
                }
                else
                {
                    missing.Add(item.Name);
                }
            }

            return new GetStreetCoordinatesListResponse()
            { Succes = streetCoordinatesList, Failed = missing };


        }



        public class GetStreetCoordinatesListResponse
        {
            public List<StreetCoordinates> Succes { get; set; }
            public List<string> Failed { get; set; }
        }

        public class StreetCoordinates
        {

            public StreetCoordinates(string name, string neighborhood, int decadeOfApprovementMeeting, bool isNamedAfterWoman, List<LatLng> coordinates)
            {
                Name = name;
                Neighborhood = neighborhood;
                Decade = decadeOfApprovementMeeting;
                IsNamedAfterWoman = isNamedAfterWoman;
                Coordinates = coordinates;
            }

            public string Name { get; set; }
            public string Neighborhood { get; set; }
            public int Decade { get; set; }
            public bool IsNamedAfterWoman { get; set; }
            public List<LatLng> Coordinates { get; set; }
        }
    }
}
