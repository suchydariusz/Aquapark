namespace EntityLayer
{
    public class PriceListAttractionReport
    {
        public int id { get; set; }
        public string entry { get; set; }
        public double price { get; set; }

        public PriceListAttractionReport() {}
        public PriceListAttractionReport(int i, string e, double p)
        {
            id = i;
            entry = e;
            price = p;
        }
    }
}
