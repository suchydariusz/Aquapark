using System;

namespace EntityLayer
{
    public class GateHistory
    {
        public int id { get; set; }
        public DateTime timestamp { get; set; }
        public int idGate { get; set; }
        public int idVisit { get; set; }
    }
}
