using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class PlannedPrices
    {
        public int price { get; set; }
        public string beginDate { get; set; }
        public string endDate { get; set; }

        public PlannedPrices(int p, string b, string e)
        {
            price = p;
            beginDate = b;
            endDate = e;
        }
    }
}
