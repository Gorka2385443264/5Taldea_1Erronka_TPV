using erronka1_talde5_tpv;
using System.Collections.Generic;

public class EskaeraGrupo
{
    public int EskaeraId { get; set; }
    public List<EskaeraDetalle> Platos { get; set; }
    public decimal PrecioTotal { get; set; }
}