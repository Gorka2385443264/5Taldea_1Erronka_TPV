using System;

namespace erronka1_talde5_tpv
{
    public class EskaeraEntity
    {
        public virtual int Id { get; set; } // Clave primaria
        public virtual int Ordainduta { get; set; } // Estado de pago (0 = no pagada, 1 = pagada)
    }
}