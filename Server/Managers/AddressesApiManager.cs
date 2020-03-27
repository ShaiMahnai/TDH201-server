using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Objects;
using static Server.Objects.GeoCodeResponse.Result.Geometry;

namespace Server.Managers
{
    class AddressesApiManager
    {
        HttpRequestsManager oHttpRequestsManager;

        string URL = "https://opendataprod.br7.org.il/dataset/37e375d0-2e8b-4698-b979-f7dd882f7f18/resource/a117e3a7-5315-4f65-9417-a629df7b5acc/download/addresses.json";
        public AddressesApiManager()
        {
            oHttpRequestsManager = new HttpRequestsManager();
        }

        private List<AddressApiResponse> GetAllAddress()
        {
            try
            {
                string response = "";
                //string response = System.IO.File.ReadAllText(@"C:\Users\shaik\Downloads\addresses.json");

                Guid g = Guid.NewGuid();
                string GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");
                Uri uri = new Uri(URL + "?email=" + GuidString + "@gmail.com", UriKind.Absolute);
                //Uri uri = new Uri(address, UriKind.Absolute);

                //request.UserAgent = "DHC 1 contact mahnai@post.bgu.ac.il";
                bool success = false;
                while (!success)
                {
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                        success = true;
                        response = oHttpRequestsManager.Get(request);

                    }
                    catch (Exception e)
                    {
                    }
                }
             
                response = response.Replace("}{", "},{");
                var arrayOfAddress = JsonConvert.DeserializeObject<AddressApiResponse[]>(response);
                var res = arrayOfAddress.ToList();
                res.RemoveAll(x => string.IsNullOrEmpty(x.streetName) || int.TryParse(x.streetName, out int a));
                res = res.OrderBy(x => x.streetName).ToList();
                return res;

            }
            catch (Exception e)
            {
            }
            return null;

        }

        public void SaveAllStreetscoordinates()
        {
            try
            {
                List<AddressApiResponse> addressApiResponse = GetAllAddress();
                List<StreetAddress> StreetAddressList = new List<StreetAddress>();
                bool first = true;

                foreach (var address in addressApiResponse)
                {
                    var streetName = address.streetName;
                    int index = StreetAddressList.FindIndex(x => x.StreetName == streetName);
                    if (index == -1)
                    {
                        if (!first)
                        {
                            //write the prev street to file
                            StreetAddress lst = StreetAddressList.Last();
                            try
                            {
                                //order list of coordinate by house number
                                lst.StreetAddresses = lst.StreetAddresses.OrderBy(x => x.Item1).ToList();

                                string json = JsonConvert.SerializeObject(lst.StreetAddresses, Formatting.Indented);
                                File.WriteAllText(@"C:/Users/shaik/OneDrive/מסמכים/studying/university/semester 9/dhc/streetsAddresses/" + lst.StreetName.Replace("\"", "").Replace("*", "") + ".json", json);

                            }
                            catch (Exception e)
                            {
                            }
                        }
                        else
                        {
                            first = false;
                        }


                        StreetAddressList.Add(new StreetAddress() { StreetName = streetName, StreetAddresses = new List<Tuple<int, LatLng>>() });
                        index = StreetAddressList.Count() - 1;
                    }
                    StreetAddressList[index].StreetAddresses.Add(
                                          new Tuple<int, LatLng>(
                                              Convert.ToInt32(address.HouseNuber),
                                              new LatLng()
                                              {
                                                  lat = Convert.ToDouble(address.lat),
                                                  lng = Convert.ToDouble(address.lon)
                                              }));

                }

            }
            catch (Exception e)
            {

            }

        }

        public class StreetAddress
        {
            public string StreetName { get; set; }

            public List<Tuple<int, LatLng>> StreetAddresses { get; set; }
        }
    }


}
