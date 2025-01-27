using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace erronka1_talde5_tpv
{
    public class EskaeraDetalle
    {
        public int EskaeraId { get; set; } // ID de la eskaera
        public int PlatoId { get; set; } // ID del plato
        public string Nota { get; set; } // Notas adicionales
        public DateTime Hora { get; set; } // Hora de creación
        public decimal Precio { get; set; } // Precio del plato
    }
}
