using System;

namespace erronka1_talde5_tpv
{
    public class EskaeraPlatera
    {
        public virtual int Id { get; set; }
        public virtual Eskaera Eskaera { get; set; } // Relación con la clase Eskaera
        public virtual Platera Platera { get; set; } // Relación con la clase Platera
        public virtual string NotaGehigarriak { get; set; } // Nota adicional
        public virtual DateTime EskaeraOrdua { get; set; } // Fecha y hora del pedido
        public virtual DateTime? AteratzeOrdua { get; set; } // Fecha y hora de salida (nullable)
    }
}
