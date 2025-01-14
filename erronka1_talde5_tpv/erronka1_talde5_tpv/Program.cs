using System;
using System.Windows.Forms;

namespace erronka1_talde5_tpv
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());  // Inicializa el formulario
        }
    }
}
