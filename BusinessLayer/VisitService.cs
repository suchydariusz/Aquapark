using System;
using System.Collections.Generic;
using System.Linq;
using EntityLayer;
using DataLayer;

namespace BusinessLayer
{
    public class VisitService
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

        public static List<Gate> getGatesID()
        {
            List<Gate> gID = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                gID = (from g in db.tbl_Gates
                       select new Gate
                       {
                            id = g.ID
                       }).ToList();
            }
            return gID;
        }

        public static void insertGateEntering(int idg, int idw)
        {
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var idv = (from i in db.tbl_Visits
                           where i.IDWatch == idw && i.StopTime == null
                           select i.ID);

                var insGE = new tbl_GateHistory
                {
                    Timestamp = DateTime.Now,
                    IDGate = idg,
                    IDVisit = idv.First()
                };
                db.tbl_GateHistories.InsertOnSubmit(insGE);
                db.SubmitChanges();
            }
        }
    }
}
