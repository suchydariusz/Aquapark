using System.Collections.Generic;
using System.Linq;
using DataLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class RFIDService
    {
        public static List<RFIDWatch> getAllRFIDWatch()
        {
            List<RFIDWatch> rstat = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                rstat =
                    (from r in db.tbl_RFIDWatches
                     select new RFIDWatch
                     {
                         id = r.ID,
                         status = r.Status
                     }).ToList();
            }
            return rstat;
        }

        public static void insRFID()
        {
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var nw = new tbl_RFIDWatch
                {
                    Status = false
                };

                db.tbl_RFIDWatches.InsertOnSubmit(nw);
                db.SubmitChanges();
            }
        }

        public static void changeStatus(int id)
        {
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var update =
                       from p in db.tbl_RFIDWatches
                       where p.ID == id
                       select p;

                foreach (tbl_RFIDWatch p in update)
                {
                    p.Status ^= true;
                }
                db.SubmitChanges();
            }
        }
    }
}
