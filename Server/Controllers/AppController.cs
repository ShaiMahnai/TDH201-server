using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Server.Managers;
using TDH.Managers;
using TDH.Objects;
using static Server.Managers.StreetCoordinatesManager;
using static TDH.Managers.OpenStreetMapAPIManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Server.Controllers
{
    [Route("api/[controller]")]
    public class AppController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {

            List<StreetCoordinates> streetCoordinatesList = GetStreetsCoordinates();
            return Ok(new { results = streetCoordinatesList });

        }

        private List<StreetCoordinates> GetStreetsCoordinates()
        {
            FilesManager oFilesManager = new FilesManager();
            var json = oFilesManager.Read("streetsCoordinates.json");
            return JsonConvert.DeserializeObject<List<StreetCoordinates>>(json);
        }
        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public dynamic Post()
        {
            FilesManager oFilesManager = new FilesManager();
            StreetsApiIManager oStreetsApiIManager = new StreetsApiIManager();
            List<StreetsApiIResponse> lst = oStreetsApiIManager.GetAllStreets();
            StreetCoordinatesManager oStreetCoordinatesManager = new StreetCoordinatesManager();
            GetStreetCoordinatesListResponse getStreetCoordinatesListResponse = oStreetCoordinatesManager.GetStreetCoordinatesList(lst);
            oFilesManager.Write("streetsCoordinates.json", getStreetCoordinatesListResponse.Succes);
            if (getStreetCoordinatesListResponse.Failed.Count > 0)
            {
                oFilesManager.Write("missingStreetsCoordinates.txt", getStreetCoordinatesListResponse.Failed);

            }
            return getStreetCoordinatesListResponse;
        }


        [HttpGet("GetAvailableSelections")]
        public GetAvailableRespones GetAvailableSelections()
        {
            List<StreetCoordinates> streetsCoordinates = GetStreetsCoordinates();
            HashSet<int> decades = new HashSet<int>();
            HashSet<string> neighborhoods = new HashSet<string>();
            foreach (var item in streetsCoordinates)
            {
                if (!decades.Contains(item.Decade))
                {
                    decades.Add(item.Decade);
                }
                if (!neighborhoods.Contains(item.Neighborhood))
                {
                    neighborhoods.Add(item.Neighborhood);
                }
            }
            List<int> decadesList = decades.ToList();
            decadesList.Sort();

            List<string> neighborhoodList = neighborhoods.ToList();
            neighborhoodList.Sort();

            GetAvailableRespones getAvailableRespones = new GetAvailableRespones()
            { Decades = decadesList, Neighborhoods = neighborhoodList };
            return getAvailableRespones;
        }


        public class GetAvailableRespones
        {
            public List<int> Decades { get; set; }
            public List<string> Neighborhoods { get; set; }
        }


    }
}
