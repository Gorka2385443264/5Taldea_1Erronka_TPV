// Eskaera2.cs
using System;

namespace erronka1_talde5_tpv
{
    public class Eskaera2 // Clase renombrada a Eskaera2
    {
        public virtual int Id { get; set; }
        public virtual int EskaeraId { get; set; }
        public virtual int PlateraId { get; set; }
        public virtual string NotaGehigarriak { get; set; }
        public virtual DateTime EskaeraOrdua { get; set; }
        public virtual DateTime AteratzeOrdua { get; set; }
    }
}