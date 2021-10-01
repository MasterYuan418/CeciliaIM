using CeciliaIM.Server.Backend;
using CeciliaIM.Server.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CeciliaIM.Server
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LogUtil.InitLog();
            LogUtil.WriteVerbose("Logutil module init successful.");
            LogUtil.WriteInformation("------ CeciliaIM Server Program start running.");
            LogUtil.WriteInformation("CeciliaIM Server Version " + About.VERSION + ",Build type:" + About.BUILD_TYPE);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
