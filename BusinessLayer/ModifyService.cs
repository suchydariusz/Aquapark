using System;
using System.Collections.Generic;
using System.Linq;
using EntityLayer;
using DataLayer;

namespace BusinessLayer
{
    public class ModifyService
    {
        public static List<PriceList> getEntry()
        {
            List<PriceList> pl = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                pl = (from r in db.tbl_PriceLists
                      select new PriceList
                      {
                          entry = r.Entry
                      }).ToList();
            }
            return pl;
        }

        public static int getPrice(string s)
        {
            int price = 0;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var pr = from ph in db.tbl_PriceHistories
                         where ph.TicketName == s && DateTime.Today >= ph.BeginDate && DateTime.Today <= ph.EndDate
                         select ph.TicketPrice;
                if(pr.Count() != 0) price = Convert.ToInt32(pr.First());
                else
                {
                    var pd = from r in db.tbl_PriceLists
                             where r.Entry == s
                             select r.Price;
                    price = Convert.ToInt32(pd.First());
                }
            }
            return price;
        }

        public static bool updateTicketPrice(string nt, double pt, DateTime ds, DateTime de)
        {
            if (ds > de)
            {
                DateTime tmp = ds;
                ds = de;
                de = tmp;
            }
            if (ds < DateTime.Today) return false;
            if (pt < 0) return false;

            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var getid =
                    from p in db.tbl_PriceLists
                    where p.Entry == nt
                    select p.ID;

                var testdate =
                    from d in db.tbl_PriceHistories
                    where d.IDPriceList == getid.First() && d.EndDate >= DateTime.Today
                    select d;

                foreach (var i in testdate)
                    if ((ds >= i.BeginDate && ds <= i.EndDate) || (de >= i.BeginDate && de <= i.EndDate) || (ds <= i.BeginDate && de >= i.EndDate))
                        return false;

                var insDate = new tbl_PriceHistory
                {
                    BeginDate = ds,
                    EndDate = de,
                    IDPriceList = getid.First(),
                    TicketName = nt,
                    TicketPrice = pt,
                };
                db.tbl_PriceHistories.InsertOnSubmit(insDate);

                db.SubmitChanges();
                return true;
            }
        }

        public static List<Attraction> getAttractionName()
        {
            List<Attraction> at = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                at = (from r in db.tbl_Attractions
                      select new Attraction
                      {
                          name = r.Name
                      }).ToList();
            }
            return at;
        }

        public static int getPriceAttraction(string s)
        {
            int price = 0;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var pr = from ah in db.tbl_AttractionHistories
                         where ah.AttractionName == s && DateTime.Today >= ah.BeginDate && DateTime.Today <= ah.EndDate
                         select ah.AttractionPrice;
                if (pr.Count() != 0) price = Convert.ToInt32(pr.First());
                else
                {
                    var pd = (from a in db.tbl_Attractions
                              join pl in db.tbl_PriceListAttractions on a.ID equals pl.IDAttraction
                              where a.Name == s
                              select pl.PriceAttraction);
                    price = Convert.ToInt32(pd.First());
                }
            }
            return price;
        }

        public static bool updatePriceAttraction(string na, double pa, DateTime ds, DateTime de)
        {
            if (ds > de)
            {
                DateTime tmp = ds;
                ds = de;
                de = tmp;
            }
            if (ds < DateTime.Today) return false;
            if (pa < 0) return false;

            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var getid =
                    from i in db.tbl_Attractions
                    where i.Name == na
                    select i.ID;

                var getidp =
                    from p in db.tbl_PriceListAttractions
                    where p.IDAttraction == getid.First()
                    select p.ID;

                var testdate =
                    from d in db.tbl_AttractionHistories
                    where d.IDAttractionList == getidp.First() && d.EndDate >= DateTime.Today
                    select d;

                foreach (var i in testdate)
                    if ((ds >= i.BeginDate && ds <= i.EndDate) || (de >= i.BeginDate && de <= i.EndDate) || (ds <= i.BeginDate && de >= i.EndDate))
                        return false;

                var insDate = new tbl_AttractionHistory
                {
                    BeginDate = ds,
                    EndDate = de,
                    IDAttractionList = getidp.First(),
                    AttractionName = na,
                    AttractionPrice = pa
                };
                db.tbl_AttractionHistories.InsertOnSubmit(insDate);

                db.SubmitChanges();
                return true;
            }
        }

        public static List<PlannedPrices> getTicketTimePeriods(string n)
        {
            List<PlannedPrices> list = new List<PlannedPrices>();
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var selection = (from pl in db.tbl_PriceLists
                           join ph in db.tbl_PriceHistories on pl.ID equals ph.IDPriceList
                           where pl.Entry == n
                           select ph);

                foreach (var i in selection)
                {
                    list.Add(new PlannedPrices(Convert.ToInt32(i.TicketPrice), i.BeginDate.ToShortDateString(), i.EndDate.ToShortDateString()));
                }
            }
            return list;
        }

        public static List<PlannedPrices> getAttractionTimePeriods(string n)
        {
            List<PlannedPrices> list = new List<PlannedPrices>();
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var selection = (from a in db.tbl_Attractions
                                 join pl in db.tbl_PriceListAttractions on a.ID equals pl.IDAttraction
                                 join ph in db.tbl_AttractionHistories on pl.ID equals ph.IDAttractionList
                                 orderby ph.EndDate ascending
                                 where a.Name == n
                                 select ph);

                foreach (var i in selection)
                {
                    list.Add(new PlannedPrices(Convert.ToInt32(i.AttractionPrice), i.BeginDate.ToShortDateString(), i.EndDate.ToShortDateString()));
                }
            }
            return list;
        }
    }
}
