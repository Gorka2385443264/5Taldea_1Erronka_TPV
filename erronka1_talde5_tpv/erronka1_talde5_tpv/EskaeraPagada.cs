namespace erronka1_talde5_tpv
{
    public class EskaeraPagada
    {
        public int EskaeraId { get; set; } // El ID de la comanda
        public int Ordainduta { get; set; } // El estado de pago (1 para pagada)

        // Constructor
        public EskaeraPagada(int eskaeraId)
        {
            EskaeraId = eskaeraId;
            Ordainduta = 1; // Asumimos que se paga en este punto
        }
    }
}
