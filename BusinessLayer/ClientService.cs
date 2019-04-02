using System;
using System.Collections.Generic;
using System.Linq;
using EntityLayer;
using DataLayer;

namespace BusinessLayer
{
    public class ClientService
    {
        public static bool checkClient(string p)
        {
            bool isThere;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var check =
                    (from c in db.tbl_Clients
                     where c.PESEL == p
                     select c).Any();
                isThere = check;
            }
            return isThere;
        }
        public static void addClient(string n, string s, string p)
        {
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var nc = new tbl_Client
                {
                    Name = n,
                    Surname = s,
                    PESEL = p
                };

                db.tbl_Clients.InsertOnSubmit(nc);
                db.SubmitChanges();
            }
        }

        public static void addPass(string p)
        {
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var sel =
                    from s in db.tbl_Clients
                    where s.PESEL == p
                    select s;

                var ap = new tbl_Pass
                {
                    WhenEnds = DateTime.Today.AddDays(30),
                    IDClient = sel.First().ID
                };

                db.tbl_Passes.InsertOnSubmit(ap);
                db.SubmitChanges();
            }
        }

        public static bool checkPass(string p)
        {
            List<Pass> pl = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var searchId =
                    from s in db.tbl_Clients
                    where s.PESEL == p
                    select s.ID;

                pl =
                    (from c in db.tbl_Passes
                     where c.ID == searchId.First()
                     select new Pass
                     {
                         whenEnds = c.WhenEnds
                     }).ToList();
            }
            if (pl.All(List => List.whenEnds >= DateTime.Today) && pl.Count() != 0) return true;
            return false;
        }

        public static void addVisitPass(int iw, int ip)
        {
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var insVis = new tbl_Visit
                {
                    StartTime = DateTime.Now,
                    StopTime = null,
                    IDWatch = iw,
                    IDPriceEntry = null,
                    IDPass = ip
                };

                db.tbl_Visits.InsertOnSubmit(insVis);
                db.SubmitChanges();
            }
        }

        public static List<RFIDWatch> getAllRFIDWatchIDPass()
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

        public static int getPassID(string p)
        {
            int i = 0;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var searchIdClient =
                    from s in db.tbl_Clients
                    where s.PESEL == p
                    select s.ID;

                var searchIdPass =
                    from pa in db.tbl_Passes
                    where pa.IDClient == searchIdClient.First()
                    select pa.ID;

                i = searchIdPass.First();
            }
            return i;
        }
    }
}
