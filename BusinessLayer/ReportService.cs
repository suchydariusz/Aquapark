using System;
using System.Linq;
using DataLayer;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using EntityLayer;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace BusinessLayer
{
    public class ReportService
    {
        public static void makeChoice(object si, Selector l, DateTime f, DateTime t)
        {
            List<string> lball = new List<string>();
            foreach (var i in l.Items) lball.Add(i.ToString());

            string s = si.ToString();
            if (s == lball[0]) reportPassHolders();
            else if (s == lball[1]) reportRFIDs();
            else if (s == lball[2]) reportCurrentPriceList();
            else if (s == lball[3]) reportVisits(f, t);
            else if (s == lball[4]) reportAttractionIntensity(f, t);
            else if (s == lball[5]) reportIncome(f, t);
        }

        private static void reportPassHolders()
        {
            List<Client> cl = null;
            List<List<Pass>> ps = new List<List<Pass>>();
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                cl =
                    (from c in db.tbl_Clients
                     select new Client
                     {
                         id = c.ID,
                         name = c.Name,
                         surname = c.Surname
                     }).ToList();

                foreach (var i in cl)
                {
                    var pp =
                        (from p in db.tbl_Passes
                         where p.IDClient == i.id
                         select new Pass
                         {
                            id = p.ID,
                            whenEnds = p.WhenEnds,
                            clientid = p.IDClient
                         }).ToList();

                    ps.Add(pp);
                }
            }

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Karnetowicze";
            List<PdfPage> pages = new List<PdfPage>();
            pages.Add(new PdfPage());
            document.AddPage(pages.Last());
            XGraphics gfx = XGraphics.FromPdfPage(pages.Last());
            XFont fontTitle = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont fontHeaders = new XFont("Verdana", 13, XFontStyle.Underline);
            XFont fontContent = new XFont("Verdana", 11, XFontStyle.Regular);
            gfx.DrawString("ZESTAWIENIE KARNETOWICZÓW", fontTitle, XBrushes.Black, new XRect(50, 50, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString(DateTime.Now.ToShortDateString(), fontContent, XBrushes.Black, new XRect(50, 80, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("ID klienta", fontHeaders, XBrushes.Black, new XRect(50, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Imię", fontHeaders, XBrushes.Black, new XRect(140, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Nazwisko", fontHeaders, XBrushes.Black, new XRect(230, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("ID karnetu", fontHeaders, XBrushes.Black, new XRect(340, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Karnet do", fontHeaders, XBrushes.Black, new XRect(430, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            int d = 160;
           
            for(int i = 0; i < cl.Count(); i++)
            {
                gfx.DrawString(cl[i].id.ToString(), fontContent, XBrushes.Black, new XRect(50, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                gfx.DrawString(cl[i].name.ToString(), fontContent, XBrushes.Black, new XRect(140, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                gfx.DrawString(cl[i].surname.ToString(), fontContent, XBrushes.Black, new XRect(230, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                foreach (var j in ps[i])
                {
                    gfx.DrawString(j.id.ToString(), fontContent, XBrushes.Black, new XRect(340, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                    gfx.DrawString(j.whenEnds.ToString("dd/MM/yyyy"), fontContent, XBrushes.Black, new XRect(430, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                    d += 20;
                    if (d > 760)
                    {
                        pages.Add(new PdfPage(document));
                        document.AddPage(pages.Last());
                        d = 50;
                        gfx = XGraphics.FromPdfPage(pages.Last());
                    }
                }
                if (ps[i].Count() == 0) d += 20;    
            }
            string filename = "aquapark-" + DateTime.Now.ToShortDateString() + "-zestawienie-karnetowiczow.pdf";
            document.Save(filename);
        }

        private static void reportRFIDs()
        {
            int working = 0;
            int notworking = 0;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                var ok =
                    from w in db.tbl_RFIDWatches
                    where w.Status == true
                    select w;

                var notok =
                    from w in db.tbl_RFIDWatches
                    where w.Status == false
                    select w;

                working = ok.Count();
                notworking = notok.Count();
            }

            string s1 = "Liczba zegarków gotowych do użycia:";
            string s2 = working.ToString();
            string s3 = "Liczba zegarków zezłomowanych:";
            string s4 = notworking.ToString();
            string s5 = "Suma:";
            string s6 = (working + notworking).ToString();

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Zegarki";
            PdfPage page1 = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page1);
            XFont fontTitle = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont fontContent = new XFont("Verdana", 11, XFontStyle.Regular);
            gfx.DrawString("STATUS ZEGARKÓW", fontTitle, XBrushes.Black, new XRect(50, 50, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString(DateTime.Now.ToString(), fontContent, XBrushes.Black, new XRect(50, 80, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString(s1, fontContent, XBrushes.Black, new XRect(50, 130, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString(s2, fontContent, XBrushes.Black, new XRect(350, 130, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString(s3, fontContent, XBrushes.Black, new XRect(50, 150, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString(s4, fontContent, XBrushes.Black, new XRect(350, 150, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString(s5, fontContent, XBrushes.Black, new XRect(50, 170, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString(s6, fontContent, XBrushes.Black, new XRect(350, 170, page1.Width, page1.Height), XStringFormats.TopLeft);
            string filename = "aquapark-" + DateTime.Now.ToShortDateString() + "-status-zegarkow.pdf";
            document.Save(filename);
        }

        private static void reportCurrentPriceList()
        {
            List<PriceList> tickets = new List<PriceList>();
            List<PriceList> ticketsdef = new List<PriceList>();
            List<PriceListAttractionReport> attractions = new List<PriceListAttractionReport>();
            List<PriceListAttractionReport> attractionsdef = new List<PriceListAttractionReport>();
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                tickets =
                    (from r in db.tbl_PriceHistories
                     where DateTime.Today >= r.BeginDate && DateTime.Today <= r.EndDate
                     select new PriceList
                     {
                         id = r.IDPriceList,
                         entry = r.TicketName,
                         price = r.TicketPrice
                     }).ToList();

                ticketsdef =
                    (from r in db.tbl_PriceLists
                     select new PriceList
                     {
                         id = r.ID,
                         entry = r.Entry,
                         price = r.Price
                     }).ToList();

                attractions =
                    (from a in db.tbl_Attractions
                     join p in db.tbl_PriceListAttractions on a.ID equals p.IDAttraction
                     join h in db.tbl_AttractionHistories on p.ID equals h.IDAttractionList
                     where DateTime.Today >= h.BeginDate && DateTime.Today <= h.EndDate
                     select new PriceListAttractionReport
                     {
                         id = a.ID,
                         entry = a.Name,
                         price = h.AttractionPrice
                     }).ToList();

                attractionsdef =
                    (from a in db.tbl_Attractions
                     join p in db.tbl_PriceListAttractions on a.ID equals p.IDAttraction
                     select new PriceListAttractionReport
                     {
                         id = a.ID,
                         entry = a.Name,
                         price = p.PriceAttraction
                     }).ToList();
            }

            foreach (var i in ticketsdef) if (tickets.All(item => item.id != i.id)) tickets.Add(i);
            tickets.Sort( (x,y) => x.id.CompareTo(y.id) );

            foreach (var i in attractionsdef) if (attractions.All(item => item.id != i.id)) attractions.Add(i);
            attractions.Sort((x, y) => x.id.CompareTo(y.id));

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Cennik";
            PdfPage page1 = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page1);
            XFont fontTitle = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont fontHeaders = new XFont("Verdana", 13, XFontStyle.Underline);
            XFont fontContent = new XFont("Verdana", 11, XFontStyle.Regular);
            gfx.DrawString("CENNIK", fontTitle, XBrushes.Black, new XRect(50, 50, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString(DateTime.Now.ToShortDateString(), fontContent, XBrushes.Black, new XRect(50, 80, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString("Nazwa", fontHeaders, XBrushes.Black, new XRect(50, 130, page1.Width, page1.Height), XStringFormats.TopLeft);
            gfx.DrawString("Cena", fontHeaders, XBrushes.Black, new XRect(350, 130, page1.Width, page1.Height), XStringFormats.TopLeft);
            int d = 160;
            foreach (var i in tickets)
            {
                gfx.DrawString(i.entry.ToString(), fontContent, XBrushes.Black, new XRect(50, d, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString(i.price.ToString(), fontContent, XBrushes.Black, new XRect(350, d, page1.Width, page1.Height), XStringFormats.TopLeft);
                d += 20;
            }
            foreach (var i in attractions)
            {
                gfx.DrawString(i.entry.ToString(), fontContent, XBrushes.Black, new XRect(50, d, page1.Width, page1.Height), XStringFormats.TopLeft);
                gfx.DrawString(i.price.ToString(), fontContent, XBrushes.Black, new XRect(350, d, page1.Width, page1.Height), XStringFormats.TopLeft);
                d += 20;
            }
            string filename = "aquapark-" + DateTime.Now.ToShortDateString() + "-aktualny-cennik.pdf";
            document.Save(filename);
        }

        private static void reportVisits(DateTime fr, DateTime to)
        {
            if (fr > to)
            {
                var h = fr;
                fr = to;
                to = h;
            }
            List<VisitReport> vr = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                vr =
                    (from v in db.tbl_Visits
                     join p in db.tbl_PriceLists on v.IDPriceEntry equals p.ID into ps
                     from p in ps.DefaultIfEmpty()
                     where v.StartTime >= fr && v.StartTime <= to.AddDays(1)
                     select new VisitReport
                     {
                         enter = v.StartTime.ToString(),
                         exit = v.StopTime.ToString(),
                         ticketType = p.Entry,
                         watchID = v.IDWatch
                     }).ToList();
            }
            foreach (var i in vr) if (i.ticketType == null) i.ticketType = "Karnet";

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Odwiedziny";
            List<PdfPage> pages = new List<PdfPage>();
            pages.Add(new PdfPage());
            document.AddPage(pages.Last());
            XGraphics gfx = XGraphics.FromPdfPage(pages.Last());
            XFont fontTitle = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont fontHeaders = new XFont("Verdana", 13, XFontStyle.Underline);
            XFont fontContent = new XFont("Verdana", 11, XFontStyle.Regular);
            gfx.DrawString("ZESTAWIENIE WIZYT", fontTitle, XBrushes.Black, new XRect(50, 50, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString(fr.ToShortDateString()+" – "+to.ToShortDateString(), fontContent, XBrushes.Black, new XRect(50, 80, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Wejście", fontHeaders, XBrushes.Black, new XRect(50, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Wyjście", fontHeaders, XBrushes.Black, new XRect(200, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Rodzaj biletu", fontHeaders, XBrushes.Black, new XRect(350, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            //gfx.DrawString("ID zegarka", fontHeaders, XBrushes.Black, new XRect(490, 130, page1.Width, page1.Height), XStringFormats.TopLeft);
            int d = 160;

            foreach (var i in vr)
            {
                gfx.DrawString(i.enter, fontContent, XBrushes.Black, new XRect(50, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                if(i.exit != null) gfx.DrawString(i.exit, fontContent, XBrushes.Black, new XRect(200, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                else gfx.DrawString("", fontContent, XBrushes.Black, new XRect(200, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                gfx.DrawString(i.ticketType, fontContent, XBrushes.Black, new XRect(350, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                //gfx.DrawString(i.watchID.ToString(), fontContent, XBrushes.Black, new XRect(490, d, page1.Width, page1.Height), XStringFormats.TopLeft);
                d += 20;
                if (d > 760)
                {
                    pages.Add(new PdfPage(document));
                    document.AddPage(pages.Last());
                    d = 50;
                    gfx = XGraphics.FromPdfPage(pages.Last());
                }
            }
            string filename = "aquapark-" + DateTime.Now.ToShortDateString() + "-zestawienie-wizyt.pdf";
            document.Save(filename);
        }

        private static void reportAttractionIntensity(DateTime fr, DateTime to)
        {
            if (fr > to)
            {
                var h = fr;
                fr = to;
                to = h;
            }
            List<AttractionIntensityReport> air = null;
            List<AttractionIntensityReport> air2 = null;
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                air = (from a in db.tbl_Attractions
                       select new AttractionIntensityReport
                       {
                           id = a.ID,
                           name = a.Name
                       }).ToList();

                air2 =
                    (from a in db.tbl_Attractions
                     join g in db.tbl_Gates on a.ID equals g.IDAttraction
                     join gh in db.tbl_GateHistories on g.ID equals gh.IDGate
                     join v in db.tbl_Visits on gh.IDVisit equals v.ID
                     where v.StartTime >= fr && v.StartTime <= to.AddDays(1)
                     select new AttractionIntensityReport
                     {
                        id = a.ID
                     }).ToList();
            }
            foreach (var i in air) i.quantity = air2.Count(item => item.id == i.id);

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Popularność atrakcji";
            List<PdfPage> pages = new List<PdfPage>();
            pages.Add(new PdfPage());
            document.AddPage(pages.Last());
            XGraphics gfx = XGraphics.FromPdfPage(pages.Last());
            XFont fontTitle = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont fontHeaders = new XFont("Verdana", 13, XFontStyle.Underline);
            XFont fontContent = new XFont("Verdana", 11, XFontStyle.Regular);
            gfx.DrawString("POPULARNOŚĆ ATRAKCJI", fontTitle, XBrushes.Black, new XRect(50, 50, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString(fr.ToShortDateString() + " – " + to.ToShortDateString(), fontContent, XBrushes.Black, new XRect(50, 80, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Nazwa", fontHeaders, XBrushes.Black, new XRect(50, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Liczba użyć", fontHeaders, XBrushes.Black, new XRect(300, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            int d = 160;

            foreach (var i in air)
            {
                gfx.DrawString(i.name, fontContent, XBrushes.Black, new XRect(50, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                gfx.DrawString(i.quantity.ToString(), fontContent, XBrushes.Black, new XRect(300, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                d += 20;
                if (d > 760)
                {
                    pages.Add(new PdfPage(document));
                    document.AddPage(pages.Last());
                    d = 50;
                    gfx = XGraphics.FromPdfPage(pages.Last());
                }
            }
            string filename = "aquapark-" + DateTime.Now.ToShortDateString() + "-popularnosc-atrakcji.pdf";
            document.Save(filename);
        }

        private static void reportIncome(DateTime fr, DateTime to)
        {
            if(fr > to)
            {
                var h = fr;
                fr = to;
                to = h;
            }
            List<IncomeReport> sourcePasses = new List<IncomeReport>();
            List<IncomeReport> sourceTickets = new List<IncomeReport>();
            List<IncomeReport> sourceAttractions = new List<IncomeReport>();
            List<IncomeReport> sourceOvertime = new List<IncomeReport>();
            using (AquaparkDBDataContext db = new AquaparkDBDataContext())
            {
                //find passes sold during the time period
                var pass = from p in db.tbl_Passes
                           where p.WhenEnds >= fr.AddDays(30) && p.WhenEnds <= to.AddDays(31)
                           select p.WhenEnds;

                //fill in passes with predefinded, determined values
                foreach (var i in pass) sourcePasses.Add(new IncomeReport("Karnet", i.AddDays(-30).ToShortDateString(), 179));

                //find tickets sold during the time period
                var ticket = from t in db.tbl_PriceLists
                             join v in db.tbl_Visits on t.ID equals v.IDPriceEntry
                             where v.StartTime >= fr && v.StartTime <= to.AddDays(1)
                             select new { t, v };

                //find ticket prices of the time
                foreach (var i in ticket)
                {
                    var whatprice = from h in db.tbl_PriceHistories
                                    where i.v.StartTime >= h.BeginDate && i.v.StartTime <= Convert.ToDateTime(h.EndDate).AddDays(1) && i.t.Entry == h.TicketName
                                    select h.TicketPrice;
                    if (whatprice.Count() != 0) sourceTickets.Add(new IncomeReport(i.t.Entry, i.v.StartTime.Value.ToShortDateString(), Convert.ToInt32(whatprice.First())));
                    else sourceTickets.Add(new IncomeReport(i.t.Entry, i.v.StartTime.Value.ToShortDateString(), -1));
                }

                //find default ticket prices if necessary
                foreach (var i in sourceTickets)
                {
                    var defprice = from t in db.tbl_PriceLists
                                   where t.Entry == i.name
                                   select t.Price;
                    if (defprice.Count() != 0) if (i.value == -1) i.value = Convert.ToInt32(defprice.First());
                }

                //find attractions uses of the time
                var att = from a in db.tbl_Attractions
                          join g in db.tbl_Gates on a.ID equals g.IDAttraction
                          join h in db.tbl_GateHistories on g.ID equals h.IDGate
                          join v in db.tbl_Visits on h.IDVisit equals v.ID
                          where v.StartTime >= fr && v.StartTime <= to.AddDays(1)
                          select new { a, g, h, v };

                //find attraction prices of the time
                foreach (var i in att)
                {
                    var whatprice = from h in db.tbl_AttractionHistories
                                    where i.v.StartTime >= h.BeginDate && i.v.StartTime <= Convert.ToDateTime(h.EndDate).AddDays(1) && i.a.Name == h.AttractionName
                                    select h.AttractionPrice;
                    if (whatprice.Count() != 0) sourceAttractions.Add(new IncomeReport(i.a.Name, i.v.StartTime.Value.ToShortDateString(), Convert.ToInt32(whatprice.First())));
                    else sourceAttractions.Add(new IncomeReport(i.a.Name, i.v.StartTime.Value.ToShortDateString(), -1));
                }

                //find default attraction prices if necessary
                foreach (var i in sourceAttractions)
                {
                    var defprice = from a in db.tbl_Attractions
                                   join p in db.tbl_PriceListAttractions on a.ID equals p.IDAttraction
                                   where a.Name == i.name
                                   select p.PriceAttraction;
                    if (defprice.Count() != 0) if (i.value == -1) i.value = Convert.ToInt32(defprice.First());
                }

                //decide overtime occurrences                
                foreach(var i in ticket)
                {
                    int howLong = 0;
                    switch (i.t.ID)
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
                    int realTime = Convert.ToInt32(Math.Ceiling((i.v.StopTime - i.v.StartTime).Value.TotalHours));
                    int exceedance = (realTime - howLong);

                    int idc = 0;
                    if (exceedance > 0)
                    {
                        switch (exceedance)
                        {
                            case 1: idc = 9; break;
                            case 2: idc = 10; break;
                            case 3: idc = 11; break;
                            default: idc = 11; break;
                        }

                        //find overtime prices of the time
                        var whatprice = from h in db.tbl_PriceHistories
                                        where i.v.StopTime >= h.BeginDate && i.v.StopTime <= Convert.ToDateTime(h.EndDate).AddDays(1) && h.IDPriceList == idc
                                        select h;
                        if (whatprice.Count() != 0) sourceOvertime.Add(new IncomeReport(whatprice.First().TicketName, i.v.StopTime.Value.ToShortDateString(), Convert.ToInt32(whatprice.First().TicketPrice)));
                        else
                        {
                            var getName = from t in db.tbl_PriceLists
                                          where t.ID == idc
                                          select t.Entry;
                            sourceOvertime.Add(new IncomeReport(getName.First().ToString(), i.v.StopTime.Value.ToShortDateString(), -1));
                        }
                    }
                }

                //find default overtime prices if necessary
                foreach (var i in sourceOvertime)
                {
                    var defprice = from t in db.tbl_PriceLists
                                   where t.Entry == i.name
                                   select t.Price;
                    if (defprice.Count() != 0) if (i.value == -1) i.value = Convert.ToInt32(defprice.First());
                }
            }

            //merge sources lists
            List<IncomeReport> sourcesNoOrder = sourcePasses.Concat(sourceTickets).Concat(sourceAttractions).Concat(sourceOvertime).ToList();
            List<IncomeReport> sources = sourcesNoOrder.OrderBy(item => item.date).ToList();
            int sum = 0;
            foreach (var i in sources) sum += i.value;

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Przychody";
            List<PdfPage> pages = new List<PdfPage>();
            pages.Add(new PdfPage());
            document.AddPage(pages.Last());
            XGraphics gfx = XGraphics.FromPdfPage(pages.Last());
            XFont fontTitle = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont fontHeaders = new XFont("Verdana", 13, XFontStyle.Underline);
            XFont fontContent = new XFont("Verdana", 11, XFontStyle.Regular);
            XFont fontSummary = new XFont("Verdana", 11, XFontStyle.Bold);
            gfx.DrawString("PRZYCHODY", fontTitle, XBrushes.Black, new XRect(50, 50, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString(fr.ToShortDateString() + " – " + to.ToShortDateString(), fontContent, XBrushes.Black, new XRect(50, 80, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Źródło", fontHeaders, XBrushes.Black, new XRect(50, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Data", fontHeaders, XBrushes.Black, new XRect(280, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString("Wpływ", fontHeaders, XBrushes.Black, new XRect(430, 130, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            int d = 160;

            foreach (var i in sources)
            {
                gfx.DrawString(i.name, fontContent, XBrushes.Black, new XRect(50, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                gfx.DrawString(i.date, fontContent, XBrushes.Black, new XRect(280, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                gfx.DrawString(i.value.ToString(), fontContent, XBrushes.Black, new XRect(430, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
                d += 20;
                if (d > 760)
                {
                    pages.Add(new PdfPage(document));
                    document.AddPage(pages.Last());
                    d = 50;
                    gfx = XGraphics.FromPdfPage(pages.Last());
                }
            }    
            
            gfx.DrawString("Suma", fontSummary, XBrushes.Black, new XRect(50, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            gfx.DrawString(sum.ToString(), fontSummary, XBrushes.Black, new XRect(430, d, pages.Last().Width, pages.Last().Height), XStringFormats.TopLeft);
            string filename = "aquapark-" + DateTime.Now.ToShortDateString() + "-przychody.pdf";
            document.Save(filename);
        }
    }
}
