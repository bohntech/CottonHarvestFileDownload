using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CottonHarvestDataTransferApp.Classes
{
    /// <summary>
    /// This class is used to locate HID files in a specific path
    /// </summary>
    public class FileLocator
    {
        #region private properties
        private string _searchPath = "";
        #endregion

        #region private methods
        private List<string> findFiles(string path, string extension)
        {
            List<string> matches = new List<string>();

            var directories = Directory.EnumerateDirectories(path);
            
            foreach(var file in Directory.EnumerateFiles(path))
            {
                if (file.ToLower().Trim().EndsWith(extension.ToLower().Trim()))
                {
                    matches.Add(file);
                }
            }

            foreach(var directory in directories)
            {
                matches.AddRange(findFiles(directory, extension));
            }

            return matches;
        }
        #endregion

        #region public methods
        public List<string> FindHIDFiles()
        {
            List<string> files = findFiles(_searchPath, ".txt");

            List<string> filteredFiles = new List<string>();

            foreach(var f in files)
            {
                var lines = File.ReadAllLines(f);
                if (lines.Length > 1) 
                {
                    var firstLine = lines[0].ToLower();

                    if (firstLine.Contains("module id") && firstLine.Contains("module sn") && (firstLine.Contains("machine vin") || firstLine.Contains("machine sn") || firstLine.Contains("machine pin")))
                    {
                        filteredFiles.Add(f);
                    }
                }
            }

            return filteredFiles;
        }
        #endregion

        public FileLocator(string searchDirectory)
        {
            _searchPath = searchDirectory;
        }
    }
}
