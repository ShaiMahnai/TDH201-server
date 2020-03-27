using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Objects
{
    public class StreetsApiIResponse
    {
        public string type { get; set; }
        public string Name { get; set; }
        public string title { get; set; }
        public string group { get; set; }
        public bool IsNamedAfterWoman { get; set; }
        public DateTime DateOfApprovementMeeting { get; set; }
        public string status { get; set; }
        public string neighborhood { get; set; }

    }
}
