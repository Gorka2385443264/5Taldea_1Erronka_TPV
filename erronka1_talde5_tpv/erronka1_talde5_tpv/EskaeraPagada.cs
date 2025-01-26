using System;

namespace erronka1_talde5_tpv
{
    public class EskaeraPagada
    {
        public int EskaeraId { get; set; }
        public decimal PrecioTotal { get; set; }
        public string MetodoPago { get; set; }
        public DateTime FechaPago { get; set; }
    }
}