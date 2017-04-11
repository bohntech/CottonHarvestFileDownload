using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using CottonHarvestDataTransferApp;
using CottonHarvestDataTransferApp.Logging;

namespace CottonHarvestDataTransferApp.Classes
{
    /// <summary>
    /// This class is used to ensure the application only has one instance running at any given time
    /// </summary>
    public class InstanceController : WindowsFormsApplicationBase
    {
        public InstanceController()
        {
            IsSingleInstance = true;

            StartupNextInstance += this_StartupNextInstance;
        }

        void this_StartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            Main form = (Main) this.MainForm;
            Logger.Log("INFO", "Start new instance requested");
            form.focusApplication();
        }

        protected override void OnCreateMainForm()
        {
            MainForm = new Main();
        }
    }
}
