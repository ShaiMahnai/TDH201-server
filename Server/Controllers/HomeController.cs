using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TDH.Managers;
using TDH.Objects;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public string Get()
        {
            return "running";
            //GoogleMapsAPIManager oGoogleMapsAPIManager = new GoogleMapsAPIManager();
            //string address= oGoogleMapsAPIManager.GetAddressByLatLng("31.25305167500005", "34.77048470200003");
            //string street = oGoogleMapsAPIManager.GetStreetByLatLng("31.25305167500005", "34.77048470200003");

            //AddressesApiManager oAddressesApiManager = new AddressesApiManager();
            //oAddressesApiManager.SaveAllStreetscoordinates();

            //OpenStreetMapAPIManager oOpenStreetMapAPIManager = new OpenStreetMapAPIManager();

            //StreetsApiIManager oStreetsApiIManager = new StreetsApiIManager();
            //List<StreetsApiIResponse> lst = oStreetsApiIManager.Get();
            //oOpenStreetMapAPIManager.SaveAllStreetsCoordinates(lst);
            //return true;
        }
    }
}
