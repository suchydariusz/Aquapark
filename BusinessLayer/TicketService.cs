using System;
using System.Collections.Generic;
using System.Linq;
using EntityLayer;
using DataLayer;

namespace BusinessLayer
{
    public class TicketService
    {
        public static List<RFIDWatch> getAllRFIDWatchID()
        {
            List<RFIDWatch> rID = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                rID = (from r in db.tbl_RFIDWatches
                       where r.Status == true
                       select new RFIDWatch
                       {
                           id = r.ID
                       }).ToList();
            }
            return rID;
        }

        public static int getIdTicket(string n)
        {
            var pl = 0;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var update =
                    from p in db.tbl_PriceLists
                    where p.Entry == n
                    select p.ID;
                pl = update.First();
            };
            return pl;
        }

        public static List<PriceList> getEntryTickets()
        {
            List<PriceList> pl = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                pl = (from r in db.tbl_PriceLists
                      where r.ID < 9
                      select new PriceList
                      {
                          entry = r.Entry
                      }).ToList();
            }
            return pl;
        }

        public static List<PriceList> getPriceTicket(string s)
        {
            List<PriceList> pl = null;
            List<PriceList> pp = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                pp = (from p in db.tbl_PriceHistories
                      where p.TicketName == s && DateTime.Today >= p.BeginDate && DateTime.Today <= p.EndDate
                      select new PriceList
                      {
                          price = p.TicketPrice
                      }).ToList();

                if (pp.Count() == 0)
                {
                    pl = (from r in db.tbl_PriceLists
                          where r.Entry == s
                          select new PriceList
                          {
                              price = r.Price
                          }).ToList();
                    return pl;
                }
                else return pp;
            }
        }

        public static void addVisit(int iw, int ip)
        {
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var insVis = new tbl_Visit
                {
                    StartTime = DateTime.Now,
                    StopTime = null,
                    IDWatch = iw,
                    IDPriceEntry = ip,
                    IDPass = null
                };

                db.tbl_Visits.InsertOnSubmit(insVis);
                db.SubmitChanges();
            }
        }
    }
}
