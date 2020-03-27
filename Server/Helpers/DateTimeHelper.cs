using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Helpers
{
    public static class DateTimeHelper
    {
        public static int GetDecade(DateTime date)
        {
            int year = date.Year;
            int decade = (year / 10 * 10);
            if (decade == 0)
            {

            }
            return (year / 10 * 10);
        }
        public static int GetYear(DateTime date)
        {
            int year = date.Year;
            int decade = (year / 10 * 10);
            if (decade == 0)
            {

            }
            return year;
        }
    }
}
