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
using static Server.Managers.StreetCoordinatesManager;
using static Server.Managers.ConclusionsManager.GetProjectConclusionsRespones;

namespace Server.Managers
{
    public class ConclusionsManager
    {



        public GetProjectConclusionsRespones GetProjectConclusions(List<StreetCoordinates> streetsCoordinates)
        {
            GetProjectConclusionsRespones conclusionsRespones = new GetProjectConclusionsRespones();
            HashSet<Tuple<string, int>> yearOfApprovmant = new HashSet<Tuple<string, int>>();

            foreach (var item in streetsCoordinates)
            {
                int newYearsCount = Helpers.DateTimeHelper.GetYear(item.DateOfApprovementMeeting);
                Tuple<string, int> tuple = yearOfApprovmant.Where(x => x.Item1.Equals(item.Neighborhood)).FirstOrDefault();
                if (tuple != null)
                {
                    if (!string.IsNullOrEmpty(item.Neighborhood))
                    {
                        newYearsCount += tuple.Item2;
                        yearOfApprovmant.Remove(tuple);
                    }
                }
                if (!string.IsNullOrEmpty(item.Neighborhood))
                {
                    yearOfApprovmant.Add(new Tuple<string, int>(item.Neighborhood, newYearsCount));
                }
                

                GetProjectConclusionsResponesDetails details =
                    conclusionsRespones.NeighborhoodsCount.Where(x => x.Name.Equals(item.Neighborhood)).FirstOrDefault();
                if (details == null)
                {
                    if (!string.IsNullOrEmpty(item.Neighborhood))
                    {
                        details = new GetProjectConclusionsResponesDetails(item.Neighborhood);
                        UpdateDatails(item, ref details);
                        conclusionsRespones.NeighborhoodsCount.Add(details);
                    }
                }
                else
                {
                    UpdateDatails(item, ref details);
                }
            }
            foreach (var item in yearOfApprovmant)
            {
                var neighborhood = conclusionsRespones.NeighborhoodsCount.Where(x => x.Name.Equals(item.Item1)).FirstOrDefault();
                neighborhood.Data.AverageYearOfApprovement = (item.Item2 / neighborhood.Data.TotalCount);
            }

            conclusionsRespones.NeighborhoodsCount = conclusionsRespones.NeighborhoodsCount.OrderBy(x => x.Name).ToList();
            return conclusionsRespones;
        }

        private void UpdateDatails(StreetCoordinates item, ref GetProjectConclusionsResponesDetails details)
        {
            int yearOfApprovmantSumSum = 0;
            details.Data.TotalCount++;
            if (!item.Exist)
            {
                details.Data.Plans++;
            }
            if (item.IsNamedAfterWoman)
            {
                details.Data.NamedAfterWomanCount++;
            }
        }

        public class GetProjectConclusionsRespones
        {
            public GetProjectConclusionsRespones()
            {
                NeighborhoodsCount = new List<GetProjectConclusionsResponesDetails>();
            }
            public List<GetProjectConclusionsResponesDetails> NeighborhoodsCount { get; set; }

            public class GetProjectConclusionsResponesDetails
            {
                public GetProjectConclusionsResponesDetails(string name)
                {
                    Name = name ?? throw new ArgumentNullException(nameof(name));
                    Data = new GetProjectConclusionsResponesData();
                }

                public string Name { get; set; }
                public GetProjectConclusionsResponesData Data { get; set; }
            }
            public class GetProjectConclusionsResponesData
            {
                public int TotalCount { get; set; }
                public int NamedAfterWomanCount { get; set; }
                public int Plans { get; set; }
                public double AverageYearOfApprovement { get; set; }
            }
        }
    }
}
