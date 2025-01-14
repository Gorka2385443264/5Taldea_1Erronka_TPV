using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using erronka1_talde5_tpv;



namespace erronka1_talde5_tpv
{
    internal class Langilea
    {
        public virtual int Id { get; set; }
        public virtual string Izena { get; set; }
        public virtual string Abizena { get; set; }
        public virtual string Pasahitza { get; set; }
        public virtual string Email { get; set; }
        public virtual int NivelPermisos { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime UpdatedAt { get; set; }
        public virtual DateTime? DeletedAt { get; set; }
    }
}