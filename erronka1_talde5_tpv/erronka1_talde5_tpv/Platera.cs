namespace erronka1_talde5_tpv
{
    public class Platera
    {
        public virtual int Id { get; set; }             // id
        public virtual string Izena { get; set; }      // Nombre del plato
        public virtual string Deskribapena { get; set; } // Descripción del plato
        public virtual string Mota { get; set; }       // Tipo de plato (categoría textual)
        public virtual string PlateraMota { get; set; } // Categoría específica (Edaria, Lehen_Platera, etc.)
        public virtual int Prezioa { get; set; }       // Precio del plato (entero)
        public virtual string Menu { get; set; }       // Menú al que pertenece (si aplica)
    }
}
