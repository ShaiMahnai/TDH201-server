using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.Objects;

namespace Server.Managers
{
    class StreetsApiIManager
    {
        HttpRequestsManager oHttpRequestsManager;

        public const string URL = "https://opendataprod.br7.org.il/dataset/0771dd36-7b03-46bd-8369-19ff8d09d79e/resource/ebadb702-a15f-46e5-96ee-571a1c04f8c9/download/street-names.json";
        char[] charsToTrim = { '*', ' ', '\'', '(', ')' };
        public StreetsApiIManager()
        {
            oHttpRequestsManager = new HttpRequestsManager();
        }
        public List<StreetsApiIResponse> GetAllStreets()
        {
            string response = "";
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            Uri uri = new Uri(URL + "?email=" + GuidString + "@gmail.com", UriKind.Absolute);
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


            JArray lst = JArray.Parse(response);
            List<StreetsApiIResponse> res = new List<StreetsApiIResponse>();
            StreetsApiIResponse streetsApiIResponseToAdd;
            HashSet<string> set = new HashSet<string>();
            foreach (var item in lst)
            {
                string type = (string)item["type"];
                if (type.Equals("רחוב") || type.Equals("משעול") ||
                    type.Equals("שדרות") || type.Equals("דרך") ||
                    type.Equals("סמטת"))
                {

                    streetsApiIResponseToAdd = item.ToObject<StreetsApiIResponse>();
                    string primaryname = ((string)item["primary-name"]).Trim(charsToTrim).Replace("-", " ");
                    string secondaryname = ((string)item["secondary-name"]).Contains('(') ? "" : ((string)item["secondary-name"]).Trim(charsToTrim).Replace("-", " ");
                    string title = ((string)item["title"]).Trim(charsToTrim).Replace("-", " ");
                    streetsApiIResponseToAdd.Name = string.IsNullOrEmpty(title) ? "" : title + " ";
                    streetsApiIResponseToAdd.Name += string.IsNullOrEmpty(secondaryname) ? "" : secondaryname + " ";
                    streetsApiIResponseToAdd.Name += string.IsNullOrEmpty(primaryname) ? "" : primaryname + " ";
                    streetsApiIResponseToAdd.Name = streetsApiIResponseToAdd.Name.Trim(charsToTrim);
                    if (streetsApiIResponseToAdd.Name.Split(' ').Length > 1)
                    {

                    }
                    if (!DateTime.TryParse((string)item["date-of-approvement-meeting"], out DateTime convertedDate))
                    {
                        if (!string.IsNullOrEmpty((string)item["date-of-approvement-meeting"]))
                        {

                        }
                        convertedDate = new DateTime(1948, 1, 1);
                    }
                    streetsApiIResponseToAdd.DateOfApprovementMeeting = convertedDate;
                    streetsApiIResponseToAdd.IsNamedAfterWoman = ((string)item["named-after-woman"]).Equals("כן") ? true : false;
                    res.Add(streetsApiIResponseToAdd);
                }
                else
                {
                    if (!set.Contains(type))
                        set.Add(type);
                }

            }
            return res;

        }


    }
}
