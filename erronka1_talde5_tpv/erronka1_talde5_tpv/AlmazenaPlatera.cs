using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace erronka1_talde5_tpv
{
    public class AlmazenaPlatera
    {
        public virtual int Id { get; set; }             // ID de la relación
        public virtual int PlateraId { get; set; }      // ID del plato
        public virtual int AlmazenaId { get; set; }     // ID del almacén
    }
}
