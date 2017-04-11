using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Diagnostics;

namespace CottonHarvestDataTransferApp.Uninstall
{
    class Program
    {
        //short exe to remove startup key created by application
        static void Main(string[] args)
        {            
            try
            {
                //check for a command line argument this
                //prevents someone from launching by just clicking the executable file
                //should only be launched as custom action from uninstaller
                if (args.Length == 1 && args[0] == "1")
                {
                    var shortcutFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "CottonHarvestFileDownloadUtility.lnk");                   
                    if (System.IO.File.Exists(shortcutFile))
                    {
                       System.IO.File.Delete(shortcutFile);
                    }                   

                    string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    dir = dir.TrimEnd('\\') + "\\CottonHarvestData";

                    Console.WriteLine("Deleting " + dir);

                    if (System.IO.Directory.Exists(dir))
                    {
                        System.IO.Directory.Delete(dir, true);
                    }                   
                }
                else
                {
                    Console.WriteLine("No command line argument specified.");
                }

                System.Threading.Thread.Sleep(3500);
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine("Press any key to exit");
                Console.ReadLine();
            }
        }
    }
}
