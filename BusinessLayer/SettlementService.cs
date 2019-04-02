using System;
using System.Collections.Generic;
using System.Linq;
using EntityLayer;
using DataLayer;

namespace BusinessLayer
{
    public class SettlementService
    {
        public static List<Visit> getInsideRFID()
        {
            List<Visit> rID = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                rID = (from r in db.tbl_Visits
                       where r.StopTime == null
                       select new Visit
                       {
                           idWatch = r.IDWatch
                       }).ToList();
            }
            return rID;
        }

        public static Tuple<List<object>, int> calculateCost(int idw, DateTime stop)
        {
            int overallPrice = 0;
            List<object> cost = new List<object>();
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var chT = (from t in db.tbl_Visits
                           where t.IDWatch == idw && t.StopTime == null
                           select t.IDPriceEntry);
                try
                {
                    bool x = chT.First() == null;
                }
                catch (InvalidOperationException)
                {
                    Tuple<List<object>, int> q = new Tuple<List<object>, int>(cost, -1);
                    return q;
                }

                if (chT.First() == null)
                {
                    cost.Add("Karnet");
                }
                else
                {
                    var ticketE = from t in db.tbl_PriceLists //to scalic i wywaalic xd albo najpierw cenah potem normalna
                                  where t.ID == chT.First()
                                  select t.Entry;

                    var ticketH = from h in db.tbl_PriceHistories
                                  where h.IDPriceList == chT.First() && DateTime.Today >= h.BeginDate && DateTime.Today <= h.EndDate
                                  select h.TicketPrice;

                    cost.Add(ticketE.First());
                    if (ticketH.Count() != 0) overallPrice += Convert.ToInt32(ticketH.First());
                    else
                    {
                        var ticketP = from t in db.tbl_PriceLists
                                      where t.ID == chT.First()
                                      select t.Price;
                        overallPrice += Convert.ToInt32(ticketP.First());
                    }

                    int howLong = 0;
                    switch(Convert.ToInt32(chT.First()))
                    {
                        case 1: howLong = 1; break;
                        case 2: howLong = 2; break;
                        case 3: howLong = 3; break;
                        case 4: howLong = 12; break;
                        case 5: howLong = 1; break;
                        case 6: howLong = 2; break;
                        case 7: howLong = 3; break;
                        case 8: howLong = 12; break;
                    }

                    var start = (from t in db.tbl_Visits
                               where t.IDWatch == idw && t.StopTime == null
                               select t.StartTime);

                    int totalSpent = Convert.ToInt32(Math.Ceiling((stop - start.First()).Value.TotalHours));
                    int exceedance = totalSpent - howLong;

                    int idc = 0;
                    if(exceedance > 0 )
                    {
                        switch (exceedance)
                        {
                            case 1: idc = 9; break;
                            case 2: idc = 10; break;
                            case 3: idc = 11; break;
                            default: idc = 11; break;
                        }
                    }

                    if(idc != 0)
                    {
                        var priceifplanned = from h in db.tbl_PriceHistories
                                             where idc == h.IDPriceList && DateTime.Today >= h.BeginDate && DateTime.Today <= h.EndDate
                                             select h;

                        if (priceifplanned.Count() != 0)
                        {
                            cost.Add(priceifplanned.First().TicketName);
                            overallPrice += Convert.ToInt32(priceifplanned.First().TicketPrice);
                        }
                        else
                        {
                            var pricepl = (from p in db.tbl_PriceLists
                                           where p.ID == idc
                                           select p);
                            cost.Add(pricepl.First().Entry);
                            overallPrice += Convert.ToInt32(pricepl.First().Price);
                        }    
                    }
                }

                var atn = (from gh in db.tbl_GateHistories
                           join vi in db.tbl_Visits on gh.IDVisit equals vi.ID
                           join ga in db.tbl_Gates on gh.IDGate equals ga.ID
                           join at in db.tbl_Attractions on ga.IDAttraction equals at.ID
                           join pla in db.tbl_PriceListAttractions on at.ID equals pla.IDAttraction
                           where vi.IDWatch == idw && vi.StopTime == null
                           select at.Name);

                foreach (var i in atn) cost.Add(i);

                foreach(var i in atn)
                {
                    var atpp = from h in db.tbl_AttractionHistories
                               where i == h.AttractionName && DateTime.Today >= h.BeginDate && DateTime.Today <= h.EndDate
                               select h.AttractionPrice;
                    if (atpp.Count() != 0) overallPrice += Convert.ToInt32(atpp.First());
                    else
                    {
                        var atp = from gh in db.tbl_GateHistories
                                  join vi in db.tbl_Visits on gh.IDVisit equals vi.ID
                                  join ga in db.tbl_Gates on gh.IDGate equals ga.ID
                                  join at in db.tbl_Attractions on ga.IDAttraction equals at.ID
                                  join pla in db.tbl_PriceListAttractions on at.ID equals pla.IDAttraction
                                  where vi.IDWatch == idw && vi.StopTime == null
                                  select pla.PriceAttraction;
                        overallPrice += Convert.ToInt32(atp.First());
                    }
                }  
                //??
                //foreach (var i in atp) overallPrice += Convert.ToInt32(i);
            }
            Tuple<List<object>, int> r = new Tuple<List<object>, int>(cost, overallPrice);
            return r;
        }

        public static void exitAquapark(int idw, DateTime stop)
        {
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var update =
                   from p in db.tbl_Visits
                   where p.IDWatch == idw && p.StopTime == null
                   select p;

                foreach (tbl_Visit p in update)
                {
                    p.StopTime = stop;
                }
                db.SubmitChanges();
            }
        }
    }
}
