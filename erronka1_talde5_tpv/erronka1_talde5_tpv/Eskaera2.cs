using System;

namespace erronka1_talde5_tpv
{
    internal class Eskaera2
    {
        public virtual int Id { get; set; }  // id
        public virtual int LangileaId { get; set; }  // langilea_id
        public virtual int MahailaId { get; set; }   // mahaila_id
        public virtual string Platera { get; set; }  // platera
        public virtual string Nota { get; set; }     // nota
        public virtual string Egoera { get; set; }   // egoera
        public virtual bool Done { get; set; }       // done
        public virtual DateTime? EskaeraDone { get; set; }  // EskaeraDone (puede ser null)
    }
}
