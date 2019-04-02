using System;

namespace EntityLayer
{
    public class Visit
    {
        public int id { get; set; }
        public DateTime startTime { get; set; }
        public DateTime stopTime { get; set; }
        public int idWatch { get; set; }
        public int idPriceEntry { get; set; }
        public int idPass { get; set; }
    }
}
