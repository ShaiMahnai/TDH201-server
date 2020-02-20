using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TDH.Managers
{
    class HttpRequestsManager
    {

        public string Get(string address)
        {
            string html = string.Empty;
            bool success = false;
            while (success == false)
            {

                try
                {

                    Guid g = Guid.NewGuid();
                    string GuidString = Convert.ToBase64String(g.ToByteArray());
                    GuidString = GuidString.Replace("=", "");
                    GuidString = GuidString.Replace("+", "");

                    Uri uri = new Uri(address+ "?email=" + GuidString + "@gmail.com", UriKind.Absolute);

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.AutomaticDecompression = DecompressionMethods.GZip;
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                        success = true;
                    }
                }

                catch (Exception e)
                {

                }
            }
            return html;
        }
    }
}
