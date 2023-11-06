using System;
using System.Windows.Forms;


namespace REDTR.DB.Connection
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Globals.CreateDeafaultFiles(false);

            Application.Run(new FrmDbConfig(Application.StartupPath));
        }
    }
}
