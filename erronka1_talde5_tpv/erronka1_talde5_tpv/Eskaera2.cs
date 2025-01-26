using System;

namespace erronka1_talde5_tpv
{
    internal class EskaeraPlatera
    {
        public virtual int Id { get; set; }  // id
        public virtual int EskaeraId { get; set; }  // eskaera_id
        public virtual int PlateraId { get; set; }  // platera_id
        public virtual string NotaGehigarriak { get; set; }  // nota_gehigarriak
        public virtual DateTime EskaeraOrdua { get; set; }  // eskaera_ordua
        public virtual DateTime AteratzeOrdua { get; set; }  // ateratze_ordua
    }
}
